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
    public class CheckInsController : Controller
    {
        private readonly MaxFitContext _context;

        public CheckInsController(MaxFitContext context)
        {
            _context = context;
        }

        // GET: CheckIns (Resepsiyon Ekranı)
        public async Task<IActionResult> Index()
        {
            // Son 10 girişi listele
            var lastCheckIns = await _context.CheckIns
                .Include(c => c.Member)
                .OrderByDescending(c => c.CheckInTime)
                .Take(10)
                .ToListAsync();

            return View(lastCheckIns);
        }

        // POST: CheckIns/ProcessCheckIn (Hızlı Giriş İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckIn(string memberSearch)
        {
            if (string.IsNullOrEmpty(memberSearch))
            {
                TempData["Error"] = "Please enter a Member ID or Name.";
                return RedirectToAction(nameof(Index));
            }

            // Üyeyi ID veya İsim üzerinden ara
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId.ToString() == memberSearch ||
                                          (m.FirstName + " " + m.LastName).Contains(memberSearch));

            if (member == null)
            {
                TempData["Error"] = "Member not found!";
                return RedirectToAction(nameof(Index));
            }

            // Aktiflik Kontrolü
            if (!member.IsActive)
            {
                TempData["Error"] = $"ACCESS DENIED: {member.FirstName} {member.LastName} is not active!";
                return RedirectToAction(nameof(Index));
            }

            // Check-In Kaydı Oluştur
            var checkIn = new CheckIn
            {
                MemberId = member.MemberId,
                CheckInTime = DateTime.Now,
                Source = "Reception Desk"
            };

            _context.Add(checkIn);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"WELCOME: {member.FirstName} {member.LastName}. Access granted.";
            return RedirectToAction(nameof(Index));
        }

        // GET: CheckIns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var checkIn = await _context.CheckIns
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.CheckInId == id);

            if (checkIn == null) return NotFound();

            return View(checkIn);
        }

        // GET: CheckIns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var checkIn = await _context.CheckIns
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.CheckInId == id);

            if (checkIn == null) return NotFound();

            return View(checkIn);
        }

        // POST: CheckIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkIn = await _context.CheckIns.FindAsync(id);
            if (checkIn != null)
            {
                _context.CheckIns.Remove(checkIn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckInExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}