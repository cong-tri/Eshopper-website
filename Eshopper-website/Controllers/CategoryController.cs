using Eshopper_website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EShopperContext _context;
        public CategoryController(EShopperContext context)
        {
            _context = context;
        }
        public IActionResult Index(string slug = "")
        {
            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Index");
            }
            var category = _context.Categories.Where(x => x.CAT_Slug == slug).FirstOrDefault();

            if (category == null) return RedirectToAction("Index");

            var productsByCategory = _context.Products.Include(x => x.Brand)
                .Where(p => p.CAT_ID == category.CAT_ID);
            
            return View(productsByCategory.OrderByDescending(p => p.PRO_ID).ToList());
        }
    }
}
