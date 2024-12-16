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
using FruitShop.Areas.Admin.DTOs.request;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly EShopperContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        public ProductController(EShopperContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var eShopperContext = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(await eShopperContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PRO_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
			ViewData["ProductStatus"] = Enum.GetValues(typeof(ProductStatusEnum))
                .Cast<ProductStatusEnum>()
				.Select(e => new SelectListItem
                {
					Value = ((int)e).ToString(),
					Text = e.ToString()
				}).ToList();

			ViewData["BRA_ID"] = new SelectList(
                _context.Brands
                //.Where(x => x.BRA_Status.ToString() == "Active")
                , "BRA_ID", "BRA_Name"
            );

            ViewData["CAT_ID"] = new SelectList(
                _context.Categories.Where(x => x.CAT_Status.ToString() == "Active"), "CAT_ID", "CAT_Name"
            );
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO request)
        {
            var product = new Product
            {
                CAT_ID = request.CAT_ID,
                BRA_ID = request.BRA_ID,
                PRO_Name = request.PRO_Name,
                PRO_Description = request.PRO_Description,
                PRO_Slug = request.PRO_Slug,
                PRO_Price = request.PRO_Price,
                PRO_Quantity = request.PRO_Quantity,
                PRO_CapitalPrice = request.PRO_CapitalPrice,
                PRO_Status = request.PRO_Status,
                CreatedBy = ""
            };
            if (ModelState.IsValid)
            {
                string? newImageFileName = null;

                if (request.PRO_Image != null)
                {
                    var extension = Path.GetExtension(request.PRO_Image.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "product-details", newImageFileName);
                    request.PRO_Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (newImageFileName != null) product.PRO_Image = newImageFileName;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "BRA_Name", product.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "CAT_Name", product.CAT_ID);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductStatus"] = Enum.GetValues(typeof(ProductStatusEnum))
                .Cast<ProductStatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "BRA_Name", product.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "CAT_Name", product.CAT_ID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductDTO request)
        {
            if (id != request.PRO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.CAT_ID = request.CAT_ID;
                    existingProduct.BRA_ID = request.BRA_ID;
                    existingProduct.PRO_Name = request.PRO_Name;
                    existingProduct.PRO_Price = request.PRO_Price;
                    existingProduct.PRO_Status = request.PRO_Status;
                    existingProduct.PRO_Slug = request.PRO_Slug;
                    existingProduct.PRO_Quantity = request.PRO_Quantity;
                    existingProduct.PRO_CapitalPrice = request.PRO_CapitalPrice;
                    existingProduct.PRO_Description = request.PRO_Description;

                    if (request.PRO_Image != null)
                    {
                        if (!string.IsNullOrEmpty(existingProduct.PRO_Image))
                        {
                            var oldFilePath = Path.Combine(_hostEnv.WebRootPath, "images", "product-details", existingProduct.PRO_Image);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        var extension = Path.GetExtension(request.PRO_Image.FileName);
                        var newImageFileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "product-details", newImageFileName);
                        request.PRO_Image.CopyTo(new FileStream(filePath, FileMode.Create));

                        existingProduct.PRO_Image = newImageFileName;
                    }

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(request.PRO_ID))
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
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "BRA_Name", request.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "CAT_Name", request.CAT_ID);
            return View(request);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PRO_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.PRO_ID == id);
        }
    }
}
