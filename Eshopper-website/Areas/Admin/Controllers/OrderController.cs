using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum.Order;
using Microsoft.AspNetCore.Authorization;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly EShopperContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(EShopperContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            try 
            {
                var orders = await _context.Orders
                    .Include(o => o.Member)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();
                
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                TempData["error"] = "Có lỗi xảy ra khi tải danh sách đơn hàng";
                return View(new List<account>());
            }
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Order details requested with null ID");
                return NotFound();
            }

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Member)
                    .Include(x => x.OrderDetails)
                    .FirstOrDefaultAsync(m => m.ORD_ID == id);

                if (order == null)
                {
                    _logger.LogWarning("Order not found with ID: {OrderId}", id);
                    return NotFound();
                }

                ViewData["OrderDetails"] = await _context.OrderDetails
                    .Include(x => x.Product)
                    .Include(x => x.Order)
                    .Where(x => x.ORD_ID == id)
                    .ToListAsync();

                ViewData["orderStatus"] = Enum.GetValues(typeof(OrderStatusEnum))
                    .Cast<OrderStatusEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }).ToList();

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for ID: {OrderId}", id);
                TempData["error"] = "Có lỗi xảy ra khi tải thông tin đơn hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int id, OrderStatusEnum statusEnum)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.ORD_ID == id);

                if (order == null)
                {
                    _logger.LogWarning("Order not found for update with ID: {OrderId}", id);
                    return NotFound();
                }

                var oldStatus = order.ORD_Status;
                order.ORD_Status = statusEnum;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = User.Identity?.Name ?? "Unknown";

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Order {OrderId} status updated from {OldStatus} to {NewStatus} by {User}",
                    id, oldStatus, statusEnum, User.Identity?.Name
                );

                return Ok(new { success = true, message = "Cập nhật trạng thái đơn hàng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status for ID: {OrderId}", id);
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra khi cập nhật trạng thái đơn hàng" });
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ORD_ID == id);
        }
    }
}
