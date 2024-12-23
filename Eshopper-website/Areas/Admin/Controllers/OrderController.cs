using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

		// GET: Admin/Category
		public async Task<IActionResult> Index()
		{
			return View(await _context.Orders.AsNoTracking().ToListAsync());
		}

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var order = await _context.Orders
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(m => m.ORD_ID == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
