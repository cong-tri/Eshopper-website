using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly EShopperContext _context;

        public CategoryController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public async Task<ActionResult> Index(int pg = 1)
        {
            List <Category> category = await _context.Categories
                .AsNoTracking()
                .Include(x => x.Products)
                .OrderBy(x => x.CAT_DisplayOrder).ToListAsync();

            const int pageSize = 10;
            if (pg > 1)
            {
                pg = 1;
            }
            int resCount = category.Count();
            
            var pager = new Paginate(resCount, pg, pageSize);
            
            int recSkip = (pg - 1) * pageSize;
            
            var data = category.Skip(recSkip).Take(pager.PageSize).ToList();
            
            ViewBag.Paper = pager;
            
            return View(data);
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CAT_ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View() ;
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                var username = userInfo != null ? userInfo.ACC_Username : "";

                category.CAT_Slug = category.CAT_Name.ToLower();
                category.CreatedBy = username;
                category.CreatedDate = DateTime.Now;

                _context.Add(category);
                await _context.SaveChangesAsync();

                TempData["success"] = "Added Category successfully !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to add category something wrong !";
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Category category)
        {
            if (id != category.CAT_ID)
            {
                TempData["error"] = "Category ID mismatch. Please try again!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                    var username = userInfo != null ? userInfo.ACC_Username : "";

                    category.CAT_Slug = category.CAT_Name.ToLower();
                    category.UpdatedBy = username;
                    category.UpdatedDate = DateTime.Now;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Category has been updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CAT_ID))
                    {
                        TempData["error"] = "A concurrency error occurred while updating the category. Please try again!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to update category. Please check the input values.";
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Invalid category ID.";
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CAT_ID == id);
            if (category == null)
            {
                TempData["error"] = $"Category with Name '{category?.CAT_Name}' was not found.";
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await HasAssociatedProducts(id))
            {
                TempData["Error"] = "Cannot delete category as it has associated products.";
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FindAsync(id);
            
            if (category != null)
            {
                TempData["success"] = $"Category '{category.CAT_Name}' was successfully deleted!";
                _context.Categories.Remove(category);
            }
            else
            {
                TempData["error"] = $"Category with ID {id} was not found.";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

		private async Task<bool> HasAssociatedProducts(int categoryId)
		{
			return await _context.Products.AnyAsync(p => p.CAT_ID == categoryId);
		}
		private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CAT_ID == id);
        }
    }
}
