using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class WishlistController : Controller
    {
        private readonly EShopperContext _context;

        public WishlistController(EShopperContext context)
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

            var wishlistItems = await _context.Wishlists.AsNoTracking()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.MEM_ID == userInfo.MEM_ID);

            return View(wishlistItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId, string returnUrl = null)
        {
			var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

			if (userInfo == null)
			{
				return RedirectToAction("Login", "User", new { Area = "Admin" });
			}

			// Check if product exists
			var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Product");
            }

            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.PRO_ID == productId && w.MEM_ID == userInfo.MEM_ID);

            if (existingItem == null)
            {
                var wishlistItem = new Wishlist
                {
                    PRO_ID = productId,
                    MEM_ID = userInfo.MEM_ID
                };

                _context.Wishlists.Add(wishlistItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product has been added to your wishlist successfully!";
            }
            else
            {
                TempData["Info"] = "This product is already in your wishlist!";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int wishId)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(wishId);
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product has been removed from your wishlist successfully!";
            }

            return RedirectToAction("Index");
        }
    }
} 