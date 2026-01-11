using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maxfit_.Models;

namespace Maxfit_.Controllers
{
    public class CourseSessionsController : Controller
    {
        private readonly MaxFitContext _context;

        public CourseSessionsController(MaxFitContext context)
        {
            _context = context;
        }


        // GET: CourseSessions
        public async Task<IActionResult> Index()
        {
            var maxFitContext = _context.CourseSessions
                .Include(c => c.Class)
                .Include(c => c.Room)
                .Include(c => c.Trainer);
            return View(await maxFitContext.ToListAsync());
        }

        // GET: CourseSessions/WeeklySchedule
        public async Task<IActionResult> WeeklySchedule()
        {
            // Get current week's Monday
            var today = DateTime.Now.Date;
            var daysSinceMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
            var monday = today.AddDays(-daysSinceMonday);
            var sunday = monday.AddDays(6);

            // Get all sessions for this week
            var sessions = await _context.CourseSessions
                .Include(c => c.Class)
                .Include(c => c.Room)
                .Include(c => c.Trainer)
                .Where(c => c.StartTime >= monday && c.StartTime <= sunday.AddDays(1))
                .OrderBy(c => c.StartTime)
                .ToListAsync();

            // Group by day of week
            var scheduleByDay = sessions
                .GroupBy(s => s.StartTime.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.OrderBy(s => s.StartTime).ToList());

            ViewBag.Monday = monday;
            ViewBag.Sunday = sunday;
            ViewBag.ScheduleByDay = scheduleByDay;

            return View(sessions);
        }

        // GET: CourseSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var courseSession = await _context.CourseSessions
                .Include(c => c.Class)
                .Include(c => c.Room)
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.CourseSessionId == id);

            if (courseSession == null) return NotFound();

            return View(courseSession);
        }

        // GET: CourseSessions/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: CourseSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CourseSessionId,ClassId,RoomId,TrainerId,StartTime,EndTime,Capacity,IsCanceled,Notes")] CourseSession courseSession,
            bool repeatWeekly = false,
            int weeksToRepeat = 1)
        {
            // KRİTİK NOKTA: Formdan sadece ID'ler geldiği için, Class, Room ve Trainer nesnelerinin 
            // kendisini validation'dan muaf tutuyoruz. Bu, aldığın "Required" hatasını çözer.
            ModelState.Remove("Class");
            ModelState.Remove("Room");
            ModelState.Remove("Trainer");
            ModelState.Remove("CourseRegistrations");
            ModelState.Remove("CourseSessionPhotos");

            if (ModelState.IsValid)
            {
                if (repeatWeekly && weeksToRepeat > 1)
                {
                    // Haftalık tekrarlama aktifse, belirtilen hafta sayısı kadar session oluştur
                    var sessionsCreated = 0;

                    for (int week = 0; week < weeksToRepeat; week++)
                    {
                        var newSession = new CourseSession
                        {
                            ClassId = courseSession.ClassId,
                            RoomId = courseSession.RoomId,
                            TrainerId = courseSession.TrainerId,
                            StartTime = courseSession.StartTime.AddDays(7 * week),
                            EndTime = courseSession.EndTime.AddDays(7 * week),
                            Capacity = courseSession.Capacity,
                            IsCanceled = courseSession.IsCanceled,
                            Notes = courseSession.Notes
                        };

                        _context.Add(newSession);
                        sessionsCreated++;
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Successfully created {sessionsCreated} weekly sessions!";
                }
                else
                {
                    // Tekli session oluştur
                    _context.Add(courseSession);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Session created successfully!";
                }

                // Kayıt başarılıysa doğrudan ana Dashboard'a yönlendiriyoruz
                return RedirectToAction("Index", "Home");
            }

            // Hata varsa listeleri tekrar doldurup Create sayfasını geri döndürür
            PopulateDropDownLists(courseSession);
            return View(courseSession);
        }

        // POST: CourseSessions/CreateWeekly (New method for weekly day-based scheduling)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeekly(
            int ClassId,
            int RoomId,
            int TrainerId,
            int Capacity,
            string startTime,
            string endTime,
            List<int> selectedDays,
            int weeksToRepeat,
            string? Notes)
        {
            if (selectedDays == null || !selectedDays.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one day of the week!";
                PopulateDropDownLists();
                return View("Create");
            }

            try
            {
                var sessionsToCreate = new List<CourseSession>();
                var today = DateTime.Now.Date;
                var conflicts = new List<string>();

                // Get next Monday as starting point
                var daysSinceMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
                var nextMonday = today.AddDays(-daysSinceMonday);

                // First, prepare all sessions to check for conflicts
                for (int week = 0; week < weeksToRepeat; week++)
                {
                    foreach (var dayOfWeek in selectedDays)
                    {
                        // Calculate the date for this day in this week
                        var sessionDate = nextMonday.AddDays(week * 7 + (dayOfWeek == 0 ? 6 : dayOfWeek - 1));

                        // Combine date with time
                        var startDateTime = DateTime.Parse($"{sessionDate:yyyy-MM-dd} {startTime}");
                        var endDateTime = DateTime.Parse($"{sessionDate:yyyy-MM-dd} {endTime}");

                        var newSession = new CourseSession
                        {
                            ClassId = ClassId,
                            RoomId = RoomId,
                            TrainerId = TrainerId,
                            StartTime = startDateTime,
                            EndTime = endDateTime,
                            Capacity = Capacity,
                            IsCanceled = false,
                            Notes = Notes
                        };

                        sessionsToCreate.Add(newSession);
                    }
                }

                // Check for conflicts with existing sessions
                foreach (var session in sessionsToCreate)
                {
                    // Check ROOM conflicts
                    var roomConflicts = await _context.CourseSessions
                        .Where(cs => cs.RoomId == session.RoomId &&
                                   cs.StartTime.Date == session.StartTime.Date &&
                                   ((cs.StartTime < session.EndTime && cs.EndTime > session.StartTime)))
                        .Include(cs => cs.Class)
                        .Include(cs => cs.Room)
                        .ToListAsync();

                    if (roomConflicts.Any())
                    {
                        foreach (var conflict in roomConflicts)
                        {
                            conflicts.Add($"Room '{conflict.Room?.Name}' conflict on {session.StartTime:dd/MM/yyyy HH:mm} - Already booked for '{conflict.Class?.Name}'");
                        }
                    }

                    // Check TRAINER conflicts
                    var trainerConflicts = await _context.CourseSessions
                        .Where(cs => cs.TrainerId == session.TrainerId &&
                                   cs.StartTime.Date == session.StartTime.Date &&
                                   ((cs.StartTime < session.EndTime && cs.EndTime > session.StartTime)))
                        .Include(cs => cs.Class)
                        .Include(cs => cs.Trainer)
                        .ToListAsync();

                    if (trainerConflicts.Any())
                    {
                        foreach (var conflict in trainerConflicts)
                        {
                            conflicts.Add($"Trainer '{conflict.Trainer?.FirstName} {conflict.Trainer?.LastName}' conflict on {session.StartTime:dd/MM/yyyy HH:mm} - Already scheduled for '{conflict.Class?.Name}'");
                        }
                    }
                }

                // If there are conflicts, return error
                if (conflicts.Any())
                {
                    // Group conflicts by type
                    var roomConflictCount = conflicts.Count(c => c.Contains("Room"));
                    var trainerConflictCount = conflicts.Count(c => c.Contains("Trainer"));

                    // Get unique conflicting dates
                    var conflictDates = conflicts
                        .Select(c => c.Substring(c.IndexOf("on ") + 3, 10))
                        .Distinct()
                        .OrderBy(d => d)
                        .ToList();

                    // Build user-friendly error message
                    var errorMessage = "<div class='alert alert-danger'>";
                    errorMessage += "<h5><i class='fa fa-exclamation-triangle me-2'></i>Schedule Conflict Detected!</h5>";
                    errorMessage += "<p class='mb-2'>Cannot create sessions due to scheduling conflicts.</p>";

                    errorMessage += "<div class='mb-2'><strong>Summary:</strong></div>";
                    errorMessage += "<ul class='mb-3'>";
                    if (roomConflictCount > 0)
                        errorMessage += $"<li><strong>{roomConflictCount}</strong> room conflicts found</li>";
                    if (trainerConflictCount > 0)
                        errorMessage += $"<li><strong>{trainerConflictCount}</strong> trainer conflicts found</li>";
                    errorMessage += $"<li>Conflicts on <strong>{conflictDates.Count}</strong> different dates</li>";
                    errorMessage += "</ul>";

                    errorMessage += "<div class='mb-2'><strong>Affected Dates:</strong></div>";
                    errorMessage += "<div class='d-flex flex-wrap gap-2 mb-3'>";
                    foreach (var date in conflictDates.Take(5))
                    {
                        errorMessage += $"<span class='badge bg-warning text-dark'>{date}</span>";
                    }
                    if (conflictDates.Count > 5)
                        errorMessage += $"<span class='badge bg-secondary'>+{conflictDates.Count - 5} more</span>";
                    errorMessage += "</div>";

                    errorMessage += "<div class='alert alert-info mb-0'>";
                    errorMessage += "<strong><i class='fa fa-lightbulb me-2'></i>How to fix:</strong><br/>";
                    errorMessage += "• Choose a different room that's available<br/>";
                    errorMessage += "• Select a different trainer<br/>";
                    errorMessage += "• Change the time slot<br/>";
                    errorMessage += "• Modify the selected days";
                    errorMessage += "</div>";
                    errorMessage += "</div>";

                    TempData["ErrorMessage"] = errorMessage;
                    PopulateDropDownLists();
                    return View("Create");
                }

                // No conflicts, proceed to create sessions
                foreach (var session in sessionsToCreate)
                {
                    _context.Add(session);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully created {sessionsToCreate.Count} sessions for {weeksToRepeat} weeks!";
                return RedirectToAction("WeeklySchedule");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating sessions: {ex.Message}";
                PopulateDropDownLists();
                return View("Create");
            }
        }

        // GET: CourseSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var courseSession = await _context.CourseSessions.FindAsync(id);
            if (courseSession == null) return NotFound();

            PopulateDropDownLists(courseSession);
            return View(courseSession);
        }

        // POST: CourseSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseSessionId,ClassId,RoomId,TrainerId,StartTime,EndTime,Capacity,IsCanceled,Notes")] CourseSession courseSession)
        {
            if (id != courseSession.CourseSessionId) return NotFound();

            // Edit işlemi için de validation temizliği yapıyoruz
            ModelState.Remove("Class");
            ModelState.Remove("Room");
            ModelState.Remove("Trainer");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseSessionExists(courseSession.CourseSessionId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists(courseSession);
            return View(courseSession);
        }

        // GET: CourseSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var courseSession = await _context.CourseSessions
                .Include(c => c.Class)
                .Include(c => c.Room)
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.CourseSessionId == id);

            if (courseSession == null) return NotFound();

            return View(courseSession);
        }

        // POST: CourseSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseSession = await _context.CourseSessions.FindAsync(id);
            if (courseSession != null)
            {
                _context.CourseSessions.Remove(courseSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseSessionExists(int id)
        {
            return _context.CourseSessions.Any(e => e.CourseSessionId == id);
        }

        /// <summary>
        /// Dropdown listelerini veritabanındaki 'Name' alanına göre doldurur.
        /// </summary>
        private void PopulateDropDownLists(CourseSession? session = null)
        {
            var classes = _context.Classes?.ToList() ?? new List<Class>();
            ViewBag.ClassId = new SelectList(classes, "ClassId", "Name", session?.ClassId);

            var rooms = _context.Rooms?.ToList() ?? new List<Room>();
            ViewBag.RoomId = new SelectList(rooms, "RoomId", "Name", session?.RoomId);

            var staffList = _context.Staff?.ToList() ?? new List<Staff>();
            var trainers = staffList.Select(s => new
            {
                StaffId = s.StaffId,
                FullName = $"{s.FirstName} {s.LastName}"
            }).ToList();

            ViewBag.TrainerId = new SelectList(trainers, "StaffId", "FullName", session?.TrainerId);
        }
    }
}