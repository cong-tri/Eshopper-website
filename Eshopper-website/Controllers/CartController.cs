using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.ViewModels;
using Eshopper_website.Utils.Enum.Order;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshopper_website.Controllers
{
    public class CartController : Controller
    {
        private readonly EShopperContext _context;
		public CartController(EShopperContext context)
		{
			_context = context;
		}
		public IActionResult Index()
        {
            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItemView cartItemView = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.PRO_Quantity * x.PRO_Price)
            };
            return View(cartItemView);
        }

        public async Task<ActionResult> Checkout()
        {
			var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

			//int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value);
            //string username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

			List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItemView cartItemView = new()
            {
                GrandTotal = cartItems.Sum(x => x.PRO_Quantity * x.PRO_Price)
            };

			var orderItem = new Order()
			{
				MEM_ID = 1,
				ORD_OrderCode = Guid.NewGuid().ToString(),
				ORD_Description = "This order is ordered by " + userInfo.ACC_Username,
				ORD_Status = OrderStatusEnum.Pending,
				ORD_PaymentMethod = OrderPaymentMethodEnum.Cash,
				ORD_ShippingCost = 100,
				CreatedBy = userInfo.ACC_Username,
				ORD_TotalPrice = cartItemView.GrandTotal,
                CreatedDate = DateTime.Now,
			};

			_context.Add(orderItem);
			await _context.SaveChangesAsync();

			foreach (var item in cartItems)
			{
				var orderDetails = new OrderDetail()
				{
					ORD_ID = orderItem.ORD_ID,
					PRO_ID = item.PRO_ID,
					ORDE_Price = item.PRO_Price,
					ORDE_Quantity = item.PRO_Quantity,
					CreatedDate = DateTime.Now,
					CreatedBy = userInfo.ACC_Username,
				};

                _context.Add(orderDetails);
                await _context.SaveChangesAsync();

            }
            HttpContext.Session.Remove("Cart");

            TempData["success"] = "Order has been created successfully! Please wait for the order has been confirmed!";
			return RedirectToAction("Index", "Cart");
		}

		public async Task<ActionResult> Add(int Id)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
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

			if (carts == null) return NotFound();

			CartItem? cartItems = carts.Where(x => x.PRO_ID == Id).FirstOrDefault();

			if (cartItems?.PRO_Quantity >= 1) ++cartItems.PRO_Quantity;

            HttpContext.Session.Set("Cart", carts);

            TempData["success"] = "Increase quantity' product successfully.";

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
	}
}
