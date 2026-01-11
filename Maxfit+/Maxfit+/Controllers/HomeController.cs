using System.Diagnostics;
using Maxfit_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxfit_.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MaxFitContext _context;

        public HomeController(MaxFitContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Redirect to login if not authenticated
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login", "Account");
            }

            // Redirect Members to their own dashboard
            if (User.IsInRole("Member"))
            {
                return RedirectToAction("MemberDashboard", "Members");
            }

            // Redirect Staff to their own dashboard
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("StaffDashboard", "Staff");
            }

            // Only Admin can see this dashboard
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            // Check-in Statistics
            ViewBag.TodayCheckIns = await _context.CheckIns
                .Where(c => c.CheckInTime.Date == today)
                .CountAsync();

            // Member Statistics
            ViewBag.TotalMembers = await _context.Members.CountAsync();
            ViewBag.ActiveMembers = await _context.Members.CountAsync(m => m.IsActive);

            // Equipment Statistics
            ViewBag.TotalEquipment = await _context.Equipment.CountAsync();

            // Today's Sessions
            ViewBag.TodayActiveSessions = await _context.CourseSessions
                .Include(cs => cs.Class)
                .Where(cs => cs.StartTime.Date == today && !cs.IsCanceled)
                .OrderBy(cs => cs.StartTime)
                .ToListAsync();

            // Payment Statistics - TODAY'S REVENUE
            ViewBag.TodayRevenue = await _context.Payments
                .Where(p => p.PaymentDate.Date == today && p.Status == "Completed")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Payment Statistics - THIS MONTH
            ViewBag.MonthRevenue = await _context.Payments
                .Where(p => p.PaymentDate >= firstDayOfMonth && p.Status == "Completed")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Pending Payments Count
            ViewBag.PendingPayments = await _context.Payments
                .CountAsync(p => p.Status == "Pending");

            // Gender Statistics for Chart
            ViewBag.MaleCount = await _context.Members.CountAsync(m => m.Gender == "M");
            ViewBag.FemaleCount = await _context.Members.CountAsync(m => m.Gender == "F");

            // Recent Members (last 5)
            var recentMembers = await _context.Members
                .OrderByDescending(m => m.RegisterDate)
                .Take(5)
                .ToListAsync();

            return View(recentMembers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
