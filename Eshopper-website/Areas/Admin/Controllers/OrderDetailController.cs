using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly EShopperContext _context;
        public OrderDetailController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public IActionResult Index()
        {
            return View( _context.OrderDetails.ToList());
        }
    }
}
