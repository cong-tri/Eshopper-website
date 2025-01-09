using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
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

            var wishlistItems = await _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.Brand)
                .Include(w => w.Product)
                    .ThenInclude(p => p.Category)
                .Where(w => w.MEM_ID == memberId)
                .ToListAsync();

            return View(wishlistItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId, string returnUrl = null)
        {
            var memberId = await GetOrCreateMemberId();

            // Check if product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Product");
            }

            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.PRO_ID == productId && w.MEM_ID == memberId);

            if (existingItem == null)
            {
                var wishlistItem = new Wishlist
                {
                    PRO_ID = productId,
                    MEM_ID = memberId
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