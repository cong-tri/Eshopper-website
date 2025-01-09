using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class CompareController : Controller
    {
        private readonly EShopperContext _context;

        public CompareController(EShopperContext context)
        {
            _context = context;
        }

        private async Task<int> GetOrCreateMemberId()
        {
            // Try to get member ID from session
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId.HasValue)
            {
                return memberId.Value;
            }

            // If no member exists, create a new one
            var member = new Member
            {
                ACC_ID = 1, // Default Account ID
                ACR_ID = 1, // Default Account Role ID
                MEM_FirstName = "Guest",
                MEM_LastName = "User",
                MEM_Email = "guest@example.com",
                MEM_Phone = "0000000000",
                MEM_Address = "Guest Address",
                MEM_Status = Utils.Enum.Member.MemberStatusEnum.Active,
                MEM_Gender = Utils.Enum.Member.MemberGenderEnum.Other
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            // Store member ID in session
            HttpContext.Session.SetInt32("MemberId", member.MEM_ID);

            return member.MEM_ID;
        }

        public async Task<IActionResult> Index()
        {
            var memberId = await GetOrCreateMemberId();

            var compareItems = await _context.Compares
                .Include(c => c.Product)
                    .ThenInclude(p => p.Brand)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Where(c => c.MEM_ID == memberId)
                .ToListAsync();

            return View(compareItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCompare(int productId, string returnUrl = null)
        {
            var memberId = await GetOrCreateMemberId();

            // Check if product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Product");
            }

            // Check if product is already in compare list
            var existingItem = await _context.Compares
                .FirstOrDefaultAsync(c => c.PRO_ID == productId && c.MEM_ID == memberId);

            if (existingItem == null)
            {
                // Check if compare list has reached maximum items (e.g., 4 items)
                var compareCount = await _context.Compares
                    .CountAsync(c => c.MEM_ID == memberId);

                if (compareCount >= 4)
                {
                    TempData["Error"] = "You can only compare up to 4 products at a time!";
                }
                else
                {
                    var compareItem = new Compare
                    {
                        PRO_ID = productId,
                        MEM_ID = memberId
                    };

                    _context.Compares.Add(compareItem);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Product has been added to your compare list successfully!";
                }
            }
            else
            {
                TempData["Info"] = "This product is already in your compare list!";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCompare(int compareId)
        {
            var compareItem = await _context.Compares.FindAsync(compareId);
            if (compareItem != null)
            {
                _context.Compares.Remove(compareItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product has been removed from your compare list successfully!";
            }

            return RedirectToAction("Index");
        }
    }
} 