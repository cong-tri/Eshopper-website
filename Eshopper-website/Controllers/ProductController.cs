using Eshopper_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
	public class ProductController : Controller
	{
        private readonly EShopperContext _context;
        public ProductController(EShopperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
            ViewData["products"] = _context.Products.Include("Category").Include("Brand").ToList();
            return View();
		}
        public IActionResult Details(int Id = 0)
        {
            if (Id == 0) return RedirectToAction("Index");

            ViewData["productsById"] = _context.Products.Include("Brand")
            .Where(p => p.PRO_ID == Id).FirstOrDefault();

            ViewData["products"] = _context.Products.Include("Category").Include("Brand").ToList();

            return View();
        }
    }
}
