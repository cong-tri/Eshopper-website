using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingController : Controller
    {
        private readonly EShopperContext _context;

        public ShippingController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shippings.ToListAsync());
        }

        public async Task<IActionResult> StoreShipping(Shipping shippingModel, string phuong, string quan, string tinh, decimal price)
        {

            shippingModel.SHIP_City = tinh;
            shippingModel.SHIP_District = quan;
            shippingModel.SHIP_Ward = phuong;
            shippingModel.SHIP_Price = price;

            try
            {

                var existingShipping = await _context.Shippings
                    .AnyAsync(x => x.SHIP_City == tinh && x.SHIP_District == quan && x.SHIP_Ward == phuong);

                if (existingShipping)
                {
                    return Ok(new { duplicate = true, message = "The data have been exsisted." });
                }

                _context.Shippings.Add(shippingModel);

                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Created shipping successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding shipping.");
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            Shipping shipping = await _context.Shippings.FindAsync(Id);

            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();
            TempData["success"] = "Shipping have been deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
