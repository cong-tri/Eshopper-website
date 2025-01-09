using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.ViewModels;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Order;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class CartController : Controller
    {
        private readonly EShopperContext _context;

        public CartController(EShopperContext context)
        {
          _context = context;
        }
        
        public IActionResult Index(Shipping shipping)
        {
            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

          var shippingPriceCookie = Request.Cookies["ShippingPrice"];
          decimal shippingPrice = 0;

          if (shippingPriceCookie != null)
          {
            var shippingPriceJson = shippingPriceCookie;
            shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
          }

          var coupon_code = Request.Cookies["CouponTitle"];

          CartItemView cartItemView = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.PRO_Quantity * x.PRO_Price) + shippingPrice,
                ShippingPrice = shippingPrice,
                CouponCode = coupon_code
            };
            return View(cartItemView);
        }
        
        public async Task<ActionResult> Add(int Id)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin", url=Request.GetEncodedUrl() });
            }

            Product? product = await _context.Products.FindAsync(Id);

            List<CartItem> carts = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem? cartItems = carts.Where(x => x.PRO_ID == Id).FirstOrDefault();

            if (cartItems == null)
            {
                if(product != null) carts.Add(new CartItem(product));
                else return NotFound();
            }
            else 
            {
                cartItems.PRO_Quantity += 1;
            }

            HttpContext.Session.Set("Cart", carts);

            TempData["success"] = "Add a new product to cart successfully.";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Increase(int Id)
        {
            List<CartItem>? carts = HttpContext.Session.Get<List<CartItem>>("Cart");
            
			      var product = _context.Products.Where(x => x.PRO_ID == Id).FirstOrDefault();

            if (product == null) 
            { 
              return NotFound();
            }
            
			      if (carts == null) 
            {
              return NotFound();
            }

            CartItem? cartItems = carts.Where(x => x.PRO_ID == Id).FirstOrDefault();

            if (cartItems?.PRO_Quantity >= 1 && product.PRO_Quantity > cartItems.PRO_Quantity)
            {
              ++cartItems.PRO_Quantity;
              TempData["success"] = "Increase quantity' product successfully.";
            }
            else
            {
				      cartItems!.PRO_Quantity = product.PRO_Quantity;
              TempData["success"] = "Maximum quantity' product successfully.";
            }

            HttpContext.Session.Set("Cart", carts);
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int Id)
        {
            List<CartItem>? carts = HttpContext.Session.Get<List<CartItem>>("Cart");

            if (carts == null) return NotFound();

            CartItem? cartItems = carts.Where(x => x.PRO_ID == Id).FirstOrDefault();

            if (cartItems?.PRO_Quantity > 1) --cartItems.PRO_Quantity;
            else carts.RemoveAll(x => x.PRO_ID == Id);

            if (carts.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Remove product out of cart successfully.";
            }
            else
            {
                HttpContext.Session.Set("Cart", carts);
                TempData["success"] = "Decrease quantity' product successfully.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int Id)
        {
            List<CartItem>? carts = HttpContext.Session.Get<List<CartItem>>("Cart");

            if (carts == null) return NotFound();
            carts.RemoveAll(x => x.PRO_ID == Id);

            if (carts.Count == 0) HttpContext.Session.Remove("Cart");
            else HttpContext.Session.Set("Cart", carts);

            TempData["success"] = "Remove product out of cart successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Remove all products out of cart successfully.";
            return RedirectToAction("Index");
		}

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                TempData["error"] = "Please enter a coupon code.";
                return RedirectToAction("Index");
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.COUP_Name == couponCode && c.COUP_Status == CouponStatusEnum.Active);

            if (coupon == null)
            {
                TempData["error"] = "Invalid coupon code or coupon is not active.";
                return RedirectToAction("Index");
            }

            if (coupon.COUP_DateExpire < DateTime.Now)
            {
                TempData["error"] = "This coupon has expired.";
                return RedirectToAction("Index");
            }

            if (coupon.COUP_Quantity <= 0)
            {
                TempData["error"] = "This coupon is no longer available.";
                return RedirectToAction("Index");
            }

            // Get cart items
            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            decimal grandTotal = cartItems.Sum(x => x.PRO_Quantity * x.PRO_Price);

            // Store coupon info in session
            HttpContext.Session.SetString("CouponCode", couponCode);
            HttpContext.Session.SetDecimal("DiscountAmount", grandTotal * 0.1m); // 10% discount

            TempData["success"] = "Coupon applied successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.Remove("CouponCode");
            HttpContext.Session.Remove("DiscountAmount");
            
            TempData["success"] = "Coupon removed successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> GetShipping(Shipping shippingModel, string quan, string tinh, string phuong)
        {

          var existingShipping = await _context.Shippings
            .FirstOrDefaultAsync(x => x.SHIP_City == tinh && x.SHIP_District == quan && x.SHIP_Ward == phuong);

          decimal shippingPrice = 0;

          if (existingShipping != null)
          {
            shippingPrice = existingShipping.SHIP_Price;
          }
          else
          {
            shippingPrice = 50;
          }
          var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
          try
          {
            var cookieOptions = new CookieOptions
            {
              HttpOnly = true,
              Expires = DateTimeOffset.UtcNow.AddHours(2),
              Secure = true
            };

            Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Error adding shipping price cookie: {ex.Message}");
          }
          return Json(new { shippingPrice });
        }

        [HttpPost]
        public async Task<IActionResult> GetCoupon(Coupon couponModel, string coupon_value)
        {
          var validCoupon = await _context.Coupons
            .FirstOrDefaultAsync(x => x.COUP_Name == coupon_value && x.COUP_Quantity >= 1);

          string couponTitle = validCoupon!.COUP_Name + " | " + validCoupon?.COUP_Description;

          if (couponTitle != null)
          {
            TimeSpan remainingTime = validCoupon!.COUP_DateExpire - DateTime.Now;
            int daysRemaining = remainingTime.Days;

            if (daysRemaining >= 0)
            {
              try
              {
                var cookieOptions = new CookieOptions
                {
                  HttpOnly = true,
                  Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                  Secure = true,
                  SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
                return Ok(new { success = true, message = "Coupon applied successfully" });
              }
              catch (Exception ex)
              {
                Console.WriteLine($"Error adding apply coupon cookie: {ex.Message}");
                return Ok(new { success = false, message = "Coupon applied failed" });
              }
            }
            else
            {
              return Ok(new { success = false, message = "Coupon has expired" });
            }

          }
          else
          {
            return Ok(new { success = false, message = "Coupon not existed" });
          }

          return Json(new { CouponTitle = couponTitle });
        }

        [HttpPost]
        [Route("Cart/RemoveShippingCookie")]
        public IActionResult RemoveShippingCookie()
        {
          Response.Cookies.Delete("ShippingPrice");
          return Json(new { success = true });
        }
	}
}
