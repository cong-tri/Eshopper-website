﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Extension;
using Eshopper_website.Areas.Admin.DTOs.request;
using Microsoft.CodeAnalysis;

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
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<Product> product = await _context.Products.AsNoTracking()
                .Include(x => x.OrderDetails)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Wishlists)
                .Include(x => x.Compares)
                .ToListAsync();

            const int pageSize = 10;

            if (pg > 1)
            {
                pg = 1;
            }

            int resCount = product.Count();
            var pager = new Paginate(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = product.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Paper = pager;
            //ViewData["IsDelete"] = HasAssociatedOrderDetail

            return View(data);
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Ratings)
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
            ViewData["ProductStatus"] = new SelectList(Enum.GetValues(typeof(ProductStatusEnum))
                .Cast<ProductStatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");

            ViewData["BRA_ID"] = new SelectList(
                _context.Brands.Where(x => x.BRA_Status.ToString() == "Active"), "BRA_ID", "BRA_Name"
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
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            var product = new Product
            {
                CAT_ID = request.CAT_ID,
                BRA_ID = request.BRA_ID,
                PRO_Name = request.PRO_Name,
                PRO_Description = request.PRO_Description,
                PRO_Slug = SlugHelper.GenerateSlug(request.PRO_Name, request.PRO_ID),
                PRO_Price = request.PRO_Price,
                PRO_Quantity = request.PRO_Quantity,
                PRO_CapitalPrice = request.PRO_CapitalPrice,
                PRO_Status = request.PRO_Status,
                PRO_Sold = 0,
                CreatedBy = userInfo?.ACC_Username,
                CreatedDate = DateTime.Now
            };

            ViewData["ProductStatus"] = new SelectList(Enum.GetValues(typeof(ProductStatusEnum))
                .Cast<ProductStatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");

            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "BRA_Name", product.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "CAT_Name", product.CAT_ID);

            if (product.PRO_Price <= product.PRO_CapitalPrice)
            {
                ModelState.AddModelError("PRO_Price", "Price must be higher than Capital Price");
                return View(product);
            }
            
            if (product.PRO_Quantity < 20 && product.PRO_Status != ProductStatusEnum.LowStock)
            {
                ModelState.AddModelError("PRO_Status", "Products with quantity less than 20 must have 'LowStock' status");
                return View(product);
            }
            
            if (product.PRO_Price > 100000000)
            {
                ModelState.AddModelError("PRO_Price", "Product price cannot exceed $100,000,000");
                return View(product);
            }
            
            if (product.PRO_CapitalPrice > 100000000)
            {
                ModelState.AddModelError("PRO_CapitalPrice", "Capital price cannot exceed $500,000");
                return View(product);
            }

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
                if (newImageFileName != null) product!.PRO_Image = newImageFileName;

                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Added product successfully !";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to add product something wrong!";
            }

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
                TempData["error"] = "Product not found!";
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
                if (request.PRO_Price <= request.PRO_CapitalPrice)
                {
                    ModelState.AddModelError("PRO_Price", "Price must be higher than Capital Price");
                    return View(request);
                }

                if (request.PRO_Quantity < 20 && request.PRO_Status != ProductStatusEnum.LowStock)
                {
                    ModelState.AddModelError("PRO_Status", "Products with quantity less than 20 must have 'LowStock' status");
                    return View(request);
                }

                if (request.PRO_Price > 100000000)
                {
                    ModelState.AddModelError("PRO_Price", "Product price cannot exceed $100,000,000");
                    return View(request);
                }

                if (request.PRO_CapitalPrice > 100000000)
                {
                    ModelState.AddModelError("PRO_CapitalPrice", "Capital price cannot exceed $500,000");
                    return View(request);
                }

                try
                {
                    var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                    var username = userInfo != null ? userInfo.ACC_Username : "";

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
                    existingProduct.PRO_Slug = SlugHelper.GenerateSlug(request.PRO_Name, request.PRO_ID);
                    existingProduct.PRO_Quantity = request.PRO_Quantity;
                    existingProduct.PRO_CapitalPrice = request.PRO_CapitalPrice;
                    existingProduct.PRO_Description = request.PRO_Description;
                    existingProduct.CreatedBy = username;
                    existingProduct.UpdatedDate = DateTime.Now;
                    existingProduct.UpdatedBy = username;
                    existingProduct.PRO_Sold = existingProduct.PRO_Sold;

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

                    TempData["success"] = "Update product successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(request.PRO_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["error"] = "Failed to update product something wrong !";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BRA_ID"] = new SelectList(_context.Brands, "BRA_ID", "BRA_Name", request.BRA_ID);
            ViewData["CAT_ID"] = new SelectList(_context.Categories, "CAT_ID", "CAT_Name", request.CAT_ID);
            return View(request);
        }

		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product ID mismatch. Please try again!";
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
            if (await HasAssociatedOrderDetail(id))
            {
                TempData["Error"] = "Cannot delete product as it has associated order details.";
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.PRO_Image))
                {
                    var oldFilePath = Path.Combine(_hostEnv.WebRootPath, "images", "product-details", product.PRO_Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.PRO_ID == id);
        }

        private async Task<bool> HasAssociatedOrderDetail(int PRO_ID)
        {
            return await _context.OrderDetails.AnyAsync(p => p.PRO_ID == PRO_ID);
        }

        public async Task<ActionResult> AddQuantity(int Id)
        {
            var productQuantity = await _context.ProductQuantities.Where(pq => pq.PRO_ID == Id).ToListAsync();
            //ViewBag.ProductQuantity = await productQuantity;
            ViewData["productQuantities"] = productQuantity;
            ViewBag.Id = Id;
            return View();
        }

        //public async Task<ActionResult>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreProductQuantity(ProductQuantity productQuantity)
        {
            var product = await _context.Products.FindAsync(productQuantity.PRO_ID);
            if (product == null)
            {
                return NotFound();
            }

            product.PRO_Quantity += productQuantity.PROQ_Quantity;

            productQuantity.PROQ_Quantity = productQuantity.PROQ_Quantity;
            productQuantity.PRO_ID = productQuantity.PRO_ID;
            productQuantity.CreatedDate = DateTime.Now;


            _context.Add(productQuantity);
            await _context.SaveChangesAsync();

            TempData["success"] = "Add Success Quantity";

            return RedirectToAction("AddQuantity", "Product", new { Id = productQuantity.PRO_ID});
            }
    }
}
