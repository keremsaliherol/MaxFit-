using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maxfit_.Models;

namespace Maxfit_.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentsController : Controller
    {
        private readonly MaxFitContext _context;

        public PaymentsController(MaxFitContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index(int? memberId)
        {
            var payments = _context.Payments
                .Include(p => p.Member)
                .OrderByDescending(p => p.PaymentDate)
                .AsQueryable();

            if (memberId.HasValue)
            {
                payments = payments.Where(p => p.MemberId == memberId.Value);
                ViewBag.MemberName = await _context.Members
                    .Where(m => m.MemberId == memberId.Value)
                    .Select(m => m.FirstName + " " + m.LastName)
                    .FirstOrDefaultAsync();
            }

            return View(await payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Member)
                .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int? memberId)
        {
            LoadDropDownLists(memberId);

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                MemberId = memberId ?? 0
            };

            return View(payment);
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,MemberId,MembershipId,Amount,PaymentDate,PaymentMethod,Status,Notes,TransactionId")] Payment payment)
        {
            ModelState.Remove("Member");
            ModelState.Remove("MembershipType");

            if (ModelState.IsValid)
            {
                payment.CreatedAt = DateTime.Now;
                _context.Add(payment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Payment recorded successfully!";
                return RedirectToAction(nameof(Index), new { memberId = payment.MemberId });
            }

            LoadDropDownLists(payment.MemberId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            LoadDropDownLists(payment.MemberId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,MemberId,MembershipId,Amount,PaymentDate,PaymentMethod,Status,Notes,TransactionId,CreatedAt")] Payment payment)
        {
            if (id != payment.PaymentId) return NotFound();

            ModelState.Remove("Member");
            ModelState.Remove("MembershipType");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Payment updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadDropDownLists(payment.MemberId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Member)
                .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Payment deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to load dropdowns
        private void LoadDropDownLists(int? memberId = null)
        {
            // Get member IDs who have completed payments (any time)
            // This will hide members who have already paid from the dropdown
            var paidMemberIds = _context.Payments
                .Where(p => p.Status == "Completed")
                .Select(p => p.MemberId)
                .Distinct()
                .ToList();

            // Get members who don't have completed payments (or include the current member if editing)
            var availableMembers = _context.Members
                .Where(m => !paidMemberIds.Contains(m.MemberId) || m.MemberId == memberId)
                .OrderBy(m => m.FirstName)
                .Select(m => new
                {
                    m.MemberId,
                    FullName = m.FirstName + " " + m.LastName
                })
                .ToList();

            ViewBag.Members = new SelectList(availableMembers, "MemberId", "FullName", memberId);

            // Load Membership Types
            ViewBag.MembershipTypes = new SelectList(
                _context.MembershipTypes.OrderBy(mt => mt.Name),
                "MembershipTypeId", "Name");

            ViewBag.PaymentMethods = new SelectList(new[] { "Cash", "Credit Card", "Debit Card", "Bank Transfer", "Online Payment" });
            ViewBag.PaymentStatuses = new SelectList(new[] { "Completed", "Pending", "Failed", "Refunded" });
        }

        // API endpoint to get MembershipType price
        [HttpGet]
        public async Task<IActionResult> GetMembershipTypePrice(int membershipTypeId)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(membershipTypeId);
            if (membershipType == null)
                return Json(new { success = false });

            return Json(new { success = true, price = membershipType.Price });
        }

        // API endpoint to get Member's MembershipType
        [HttpGet]
        public async Task<IActionResult> GetMemberInfo(int memberId)
        {
            var member = await _context.Members
                .Include(m => m.MembershipType)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);

            if (member == null || member.MembershipType == null)
                return Json(new { success = false });

            return Json(new
            {
                success = true,
                membershipTypeId = member.MembershipTypeId,
                membershipTypeName = member.MembershipType.Name,
                membershipTypePrice = member.MembershipType.Price
            });
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
