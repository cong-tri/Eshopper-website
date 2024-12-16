using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
    public class BrandController : Controller
    {
        private readonly EShopperContext _context;
        public BrandController(EShopperContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index(string slug = "")
        {
            var brand = _context.Brands.Where(x => x.BRA_Slug == slug).FirstOrDefault();

            if (brand == null) return RedirectToAction("Index");

            var productsByBrand = _context.Products.Include(x => x.Category)
                .Where(p => p.BRA_ID == brand.BRA_ID);
            //return View(
            //    await _context.Products.AsNoTracking().OrderBy(x => x.PRO_Price)
            //    .Where(x => x.CAT_ID == category.CAT_ID).ToListAsync()
            //);
            return View(await productsByBrand.OrderByDescending(p => p.BRA_ID).ToListAsync());
        }
    }
}
