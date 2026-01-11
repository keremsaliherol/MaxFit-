using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxfit_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Maxfit_.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class StaffController : Controller
    {
        private readonly MaxFitContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StaffController(MaxFitContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Staff/Dashboard (for logged-in staff member)
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> StaffDashboard()
        {
            var userEmail = User.Identity?.Name;
            var staff = await _context.Staff
                .Include(s => s.CourseSessions)
                    .ThenInclude(cs => cs.Class)
                .Include(s => s.CourseSessions)
                    .ThenInclude(cs => cs.Room)
                .FirstOrDefaultAsync(s => s.Email == userEmail);

            if (staff == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Get upcoming sessions (next 30 days)
            ViewBag.UpcomingSessions = await _context.CourseSessions
                .Include(cs => cs.Class)
                .Include(cs => cs.Room)
                .Where(cs => cs.TrainerId == staff.StaffId &&
                            cs.StartTime >= DateTime.Now &&
                            cs.StartTime <= DateTime.Now.AddDays(30) &&
                            !cs.IsCanceled)
                .OrderBy(cs => cs.StartTime)
                .Take(10)
                .ToListAsync();

            // Statistics
            ViewBag.TotalSessions = await _context.CourseSessions
                .CountAsync(cs => cs.TrainerId == staff.StaffId);

            ViewBag.CompletedSessions = await _context.CourseSessions
                .CountAsync(cs => cs.TrainerId == staff.StaffId && cs.EndTime < DateTime.Now);

            ViewBag.UpcomingSessionsCount = await _context.CourseSessions
                .CountAsync(cs => cs.TrainerId == staff.StaffId &&
                                 cs.StartTime > DateTime.Now &&
                                 !cs.IsCanceled);

            return View(staff);
        }

        // GET: Staff/MyProfile (Redirects to Edit page for current staff)
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> MyProfile()
        {
            var userEmail = User.Identity?.Name;
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Email == userEmail);

            if (staff == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return RedirectToAction("Edit", new { id = staff.StaffId });
        }

        // GET: Staff (Admin view)
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Index()
        {
            var staffList = await _context.Staff.OrderByDescending(s => s.HireDate).ToListAsync();
            return View(staffList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,FirstName,LastName,Position,Phone,Email,HireDate,IsActive")] Staff staff)
        {
            // CS0029 Hatasını tamamen engellemek için doğrudan atama:
            staff.HireDate = DateOnly.FromDateTime(DateTime.Now);


            // Validasyonun HireDate yüzünden takılmasını engellemek için:
            ModelState.Remove("HireDate");

            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();

                // Auto-create user account for staff
                var userEmail = staff.Email;
                var userPassword = $"{staff.FirstName.ToLower()}123"; // Simple password format

                var user = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, userPassword);

                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                    TempData["SuccessMessage"] = $"Staff created! Login: {userEmail} / Password: {userPassword}";
                }
                else
                {
                    TempData["SuccessMessage"] = "Staff created but user account failed.";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,FirstName,LastName,Position,Phone,Email,HireDate,IsActive")] Staff staff)
        {
            if (id != staff.StaffId) return NotFound();

            // Edit işleminde HireDate'in kaybolmaması için (DateOnly güvenliği)
            if (staff.HireDate == default)
            {
                var existingStaff = await _context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.StaffId == id);
                if (existingStaff != null) staff.HireDate = existingStaff.HireDate;
            }

            ModelState.Remove("HireDate");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Staff.Any(e => e.StaffId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var staff = await _context.Staff
                .Include(s => s.CourseSessions)
                    .ThenInclude(cs => cs.Class)
                .FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null) return NotFound();

            // Get session statistics
            ViewBag.TotalSessions = await _context.CourseSessions
                .CountAsync(cs => cs.TrainerId == id);

            ViewBag.UpcomingSessions = await _context.CourseSessions
                .CountAsync(cs => cs.TrainerId == id && cs.StartTime > DateTime.Now && !cs.IsCanceled);

            return View(staff);
        }

        // GET: Staff/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null) return NotFound();

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}