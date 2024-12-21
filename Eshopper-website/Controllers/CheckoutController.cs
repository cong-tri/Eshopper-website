using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshopper_website.Controllers
{
	public class CheckoutController : Controller
	{
        private readonly EShopperContext _context;
        public CheckoutController(EShopperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Checkout()
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //if (userEmail == null)
            //{
            //    //return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    var ordercode = Guid.NewGuid().ToString();
            //    var orderItem = new Order()
            //    {
            //        MEM_ID = 1,
            //        ORD_OrderCode = ordercode,
            //        ORD_Description = "This order is created by admin in order to testing.",
            //        ORD_Status = 2,
            //        ORD_PaymentMethod = 1,
            //        ORD_ShippingCost = 100,
            //        CreatedBy = "admin",
            //        CreatedDate = DateTime.Now,
            //    };

            //    _context.Add(orderItem);
            //    await _context.SaveChangesAsync();

            //    TempData["success"] = "Order has been created successfully";

            //    return RedirectToAction("Cart", "Index");
            //}
            return View();
        }
	}
}
