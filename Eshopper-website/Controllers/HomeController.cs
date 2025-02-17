using System.Diagnostics;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly EShopperContext _context;

		public HomeController(ILogger<HomeController> logger, EShopperContext context)
        {
			_context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category).Include(x => x.Brand).ToList();
            var banners = _context.Banners.OrderBy(x => x.BAN_DisplayOrder).ToList();
            ViewData["Banner"] = banners;

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Contact"] = _context.Contacts.AsNoTracking().ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Home/Error404")]
        public IActionResult Error404()
        {
            return View("Error404");
        }

        [Route("/Login")]
        public IActionResult Login() 
        {
            return RedirectToAction("Login", "User", new {Area = "Admin"});
        }

        public IActionResult AboutUs()
        {
            return View();
        }

    }
}
