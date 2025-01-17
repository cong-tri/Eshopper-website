    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly EShopperContext _context;

        public BrandController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Brand
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.AsNoTracking()
                .Include(x => x.Products).OrderBy(x => x.BRA_DisplayOrder).ToListAsync());
        }

        // GET: Admin/Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BRA_ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Brand brand)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                var username = userInfo != null ? userInfo.ACC_Username : "";

                brand.CreatedBy = username;
                brand.CreatedDate = DateTime.Now;

                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Brand brand)
        {
            if (id != brand.BRA_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                    var username = userInfo != null ? userInfo.ACC_Username : "";

                    brand.CreatedBy = username;
                    brand.UpdatedBy = username;
                    brand.UpdatedDate = DateTime.Now;

                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BRA_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BRA_ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Admin/Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                if (await HasAssociatedProducts(id))
                {
                    TempData["Error"] = "Cannot delete brand as it has associated products.";
                    return RedirectToAction(nameof(Index));
                }
                _context.Brands.Remove(brand);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> HasAssociatedProducts(int bra_id)
        {
            return await _context.Products.AnyAsync(p => p.BRA_ID == bra_id);
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.BRA_ID == id);
        }
    }
}
