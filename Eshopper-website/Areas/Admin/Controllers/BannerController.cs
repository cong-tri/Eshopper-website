using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Areas.Admin.DTOs.request;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly EShopperContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        public BannerController(EShopperContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Banner
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banners.ToListAsync());
        }

        // GET: Admin/Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.BAN_ID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Admin/Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BannerDTO request)
        {
            var banner = new Banner
            {
                BAN_ID = request.BAN_ID,
                BAN_Title = request.BAN_Title,
                BAN_DisplayOrder = request.BAN_DisplayOrder,
                BAN_Url = request.BAN_Url,
            };

			if (ModelState.IsValid)
            {
                string? newImageFileName = null;
                if (request.BAN_Image != null)
                {
                    var extension = Path.GetExtension(request.BAN_Image.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "home", newImageFileName);
                    request.BAN_Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (newImageFileName != null) banner.BAN_Image = newImageFileName;

                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Admin/Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BannerDTO request)
        {

            if (id != request.BAN_ID)
            {
                return NotFound();
            }

            var bannerExsiting = await _context.Banners.FindAsync(id);
            if (bannerExsiting == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bannerExsiting.BAN_ID = request.BAN_ID;
                    bannerExsiting.BAN_DisplayOrder = request.BAN_DisplayOrder;
                    bannerExsiting.BAN_Title = request.BAN_Title;
                    bannerExsiting.BAN_Url = request.BAN_Url;
                    bannerExsiting.UpdatedDate = DateTime.Now;

                    if (request.BAN_Image != null)
                    {
                        if (!string.IsNullOrEmpty(bannerExsiting.BAN_Image))
                        {
                            var oldImagePath = Path.Combine(_hostEnv.WebRootPath, "images", "home", bannerExsiting.BAN_Image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var extension = Path.GetExtension(request.BAN_Image.FileName);
                        var newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "home", newImageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.BAN_Image.CopyToAsync(stream);
                        }

                        bannerExsiting.BAN_Image = newImageFileName;
                    }

                    _context.Update(bannerExsiting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(request.BAN_ID))
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
            return View(request);
        }

        // GET: Admin/Banner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.BAN_ID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Admin/Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.BAN_ID == id);
        }
    }
}
