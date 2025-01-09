using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Order;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly EShopperContext _context;

        public OrderController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            var eShopperContext = _context.Orders.Include(o => o.Member);
            return View(await eShopperContext.ToListAsync());
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Member)
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(m => m.ORD_ID == id);

            ViewData["OrderDetails"] = await _context.OrderDetails
                .Include(x => x.Product).Include(x => x.Order).Where(x => x.ORD_ID == id).ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            ViewData["orderStatus"] = Enum.GetValues(typeof(OrderStatusEnum))
                .Cast<OrderStatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            return View(order);
        }

        //public async Task<IActionResult> UpdateOrder(int id, [FromForm] Order order)
        //{
        //    if (id != order.ORD_ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //            TempData["success"] = "Order status updated successfully";
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.ORD_ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                TempData["error"] = "An error occurred while updating the order status.";
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));

        //    }
        //    return View(order);
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int id, OrderStatusEnum statusEnum)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.ORD_ID == id);

            if (order == null)
            {
                return NotFound();
            }

            order.ORD_Status = statusEnum;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the order status.");
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ORD_ID == id);
        }
    }
}
