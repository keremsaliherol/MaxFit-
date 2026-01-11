using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maxfit_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Maxfit_.Controllers
{
    // Controller seviyesinde genel yetkilendirme: Sadece giriş yapmış yetkili kullanıcılar erişebilir.
    [Authorize]
    public class MembersController : Controller
    {
        private readonly MaxFitContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public MembersController(MaxFitContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // --- ÜYE ÖZEL ALANI (Member Role Only) ---

        /// <summary>
        /// Üye giriş yaptığında kendi bilgilerini, derslerini ve üyelik durumunu gördüğü özel sayfa.
        /// </summary>
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MemberDashboard()
        {
            // Giriş yapan kullanıcının Identity e-posta adresini alıyoruz.
            var userEmail = User.Identity?.Name;

            // Veritabanından bu e-postaya ait üyeyi, ilişkili verileriyle (Üyelik Tipi, Kayıtlı Kurslar) getiriyoruz.
            var member = await _context.Members
                .Include(m => m.MembershipType)
                .Include(m => m.CourseRegistrations)
                    .ThenInclude(cr => cr.CourseSession)
                    .ThenInclude(cs => cs.Class)
                .FirstOrDefaultAsync(m => m.Email == userEmail);

            if (member == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Total Check-ins
            ViewBag.TotalCheckIns = await _context.CheckIns
                .CountAsync(c => c.MemberId == member.MemberId);

            return View(member);
        }

        // Available Classes for Member
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> AvailableClasses()
        {
            var userEmail = User.Identity?.Name;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == userEmail);

            if (member == null)
                return RedirectToAction("AccessDenied", "Account");

            // Get upcoming sessions (not canceled, not full)
            var availableSessions = await _context.CourseSessions
                .Include(cs => cs.Class)
                .Include(cs => cs.Trainer)
                .Include(cs => cs.Room)
                .Where(cs => cs.StartTime > DateTime.Now && !cs.IsCanceled)
                .OrderBy(cs => cs.StartTime)
                .Take(20)
                .ToListAsync();

            ViewBag.MemberId = member.MemberId;
            return View(availableSessions);
        }

        // Register for Class
        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> RegisterForClass(int sessionId)
        {
            var userEmail = User.Identity?.Name;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == userEmail);

            if (member == null)
                return RedirectToAction("AccessDenied", "Account");

            // Check if already registered
            var existingReg = await _context.CourseRegistrations
                .FirstOrDefaultAsync(cr => cr.MemberId == member.MemberId && cr.CourseSessionId == sessionId);

            if (existingReg != null)
            {
                TempData["ErrorMessage"] = "You are already registered for this class!";
                return RedirectToAction(nameof(AvailableClasses));
            }

            var registration = new CourseRegistration
            {
                MemberId = member.MemberId,
                CourseSessionId = sessionId,
                RegistrationDate = DateTime.Now,
                Status = "Confirmed"
            };

            _context.CourseRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Successfully registered for the class!";
            return RedirectToAction(nameof(MemberDashboard));
        }

        // --- YÖNETİM ALANI (Admin & Staff Only) ---

        // GET: Members
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .Include(m => m.MembershipType)
                .OrderByDescending(m => m.RegisterDate)
                .ToListAsync();
            return View(members);
        }

        // GET: Members/Details/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null) return NotFound();

            // Payment History (last 10)
            ViewBag.PaymentHistory = await _context.Payments
                .Where(p => p.MemberId == id)
                .OrderByDescending(p => p.PaymentDate)
                .Take(10)
                .ToListAsync();

            // Check-in History (last 15)
            ViewBag.CheckInHistory = await _context.CheckIns
                .Where(c => c.MemberId == id)
                .OrderByDescending(c => c.CheckInTime)
                .Take(15)
                .ToListAsync();

            // Statistics
            ViewBag.TotalPayments = await _context.Payments
                .Where(p => p.MemberId == id && p.Status == "Completed")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            ViewBag.TotalCheckIns = await _context.CheckIns
                .CountAsync(c => c.MemberId == id);

            return View(member);
        }

        // GET: Members/Create
        [Authorize(Roles = "Admin,Staff")]
        public IActionResult Create()
        {
            ViewBag.MembershipTypeId = new SelectList(_context.MembershipTypes, "MembershipTypeId", "Name");
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([Bind("MemberId,FirstName,LastName,Gender,BirthDate,Phone,Email,Address,RegisterDate,IsActive,MembershipTypeId")] Member member, IFormFile? PhotoFile)
        {
            // İlişkili nesneleri doğrulamadan çıkararak ModelState.IsValid'in true dönmesini sağlıyoruz.
            ModelState.Remove("MembershipType");
            ModelState.Remove("CourseRegistrations");
            ModelState.Remove("MemberCheckIns");

            if (ModelState.IsValid)
            {
                NormalizeGender(member);

                if (member.RegisterDate == default)
                    member.RegisterDate = DateTime.Now;

                // Handle photo upload
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "members");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(fileStream);
                    }

                    member.PhotoUrl = "/images/members/" + uniqueFileName;
                }

                _context.Add(member);
                await _context.SaveChangesAsync();

                // Auto-create user account for member
                // Using member's actual email address for login consistency
                var userEmail = member.Email;
                var userPassword = $"{member.FirstName.ToLower()}123"; // Simple password format

                var user = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, userPassword);

                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    TempData["SuccessMessage"] = $"Member created! Login: {userEmail} / Password: {userPassword}";
                }
                else
                {
                    TempData["SuccessMessage"] = "Member created but user account failed.";
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.MembershipTypeId = new SelectList(_context.MembershipTypes, "MembershipTypeId", "Name", member.MembershipTypeId);
            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            ViewBag.MembershipTypeId = new SelectList(_context.MembershipTypes, "MembershipTypeId", "Name", member.MembershipTypeId);
            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,FirstName,LastName,Gender,BirthDate,Phone,Email,Address,RegisterDate,IsActive,MembershipTypeId,PhotoUrl")] Member member, IFormFile? profilePhoto)
        {
            if (id != member.MemberId) return NotFound();

            ModelState.Remove("MembershipType");
            ModelState.Remove("CourseRegistrations");
            ModelState.Remove("MemberCheckIns");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle photo upload
                    if (profilePhoto != null && profilePhoto.Length > 0)
                    {
                        var photoUrl = await SaveProfilePhoto(profilePhoto, member.MemberId);
                        member.PhotoUrl = photoUrl;
                    }

                    NormalizeGender(member);
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Member updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MembershipTypeId = new SelectList(_context.MembershipTypes, "MembershipTypeId", "Name", member.MembershipTypeId);
            return View(member);
        }

        // GET: Members/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void NormalizeGender(Member member)
        {
            if (!string.IsNullOrEmpty(member.Gender))
            {
                if (member.Gender.StartsWith("M", StringComparison.OrdinalIgnoreCase)) member.Gender = "M";
                else if (member.Gender.StartsWith("F", StringComparison.OrdinalIgnoreCase)) member.Gender = "F";
            }
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        // --- MEMBER PROFILE UPDATE (Member self-service) ---

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UpdateProfile()
        {
            var userEmail = User.Identity?.Name;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == userEmail);

            if (member == null) return RedirectToAction("AccessDenied", "Account");

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UpdateProfile([Bind("MemberId,FirstName,LastName,Phone,Email,Address,PhotoUrl")] Member member, IFormFile? profilePhoto)
        {
            var userEmail = User.Identity?.Name;
            var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.Email == userEmail);

            if (existingMember == null || existingMember.MemberId != member.MemberId)
                return RedirectToAction("AccessDenied", "Account");

            ModelState.Remove("MembershipType");
            ModelState.Remove("CourseRegistrations");
            ModelState.Remove("CheckIns");
            ModelState.Remove("Memberships");
            ModelState.Remove("MemberNotifications");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle photo upload
                    if (profilePhoto != null && profilePhoto.Length > 0)
                    {
                        var photoUrl = await SaveProfilePhoto(profilePhoto, member.MemberId);
                        existingMember.PhotoUrl = photoUrl;
                    }

                    // Update allowed fields
                    existingMember.FirstName = member.FirstName;
                    existingMember.LastName = member.LastName;
                    existingMember.Phone = member.Phone;
                    existingMember.Address = member.Address;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(MemberDashboard));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId)) return NotFound();
                    else throw;
                }
            }

            return View(member);
        }

        // Helper method to save profile photo
        private async Task<string> SaveProfilePhoto(IFormFile photo, int memberId)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"member_{memberId}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            return $"/uploads/profiles/{uniqueFileName}";
        }
    }
}