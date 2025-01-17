using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Http.Extensions;
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

        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin", url = Request.GetEncodedUrl() });
            }

            var compareItems = await _context.Compares
                .Include(c => c.Product)
                    .ThenInclude(p => p.Brand)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Where(c => c.MEM_ID == userInfo.MEM_ID)
                .ToListAsync();

            return View(compareItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCompare(int productId, string returnUrl = null)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin", url = Request.GetEncodedUrl() });
            }
            // Check if product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Product");
            }

            // Check if product is already in compare list
            var existingItem = await _context.Compares
                .FirstOrDefaultAsync(c => c.PRO_ID == productId && c.MEM_ID == userInfo.MEM_ID);

            if (existingItem == null)
            {
                // Check if compare list has reached maximum items (e.g., 4 items)
                var compareCount = await _context.Compares
                    .CountAsync(c => c.MEM_ID == userInfo.MEM_ID);

                if (compareCount >= 4)
                {
                    TempData["Error"] = "You can only compare up to 4 products at a time!";
                }
                else
                {
                    var compareItem = new Compare
                    {
                        PRO_ID = productId,
                        MEM_ID = userInfo.MEM_ID
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