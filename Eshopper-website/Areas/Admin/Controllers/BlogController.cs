using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Utils.Extension;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly EShopperContext _context;
        private readonly IWebHostEnvironment _hostEnv;

        public BlogController(EShopperContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }

        // GET: Admin/Blog
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        // GET: Admin/Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BLG_ID == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BlogDTO request)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            var blog = new Blog
            {
                BLG_ID = request.BLG_ID,
                BLG_AuthorName = request.BLG_AuthorName,
                BLG_Content = request.BLG_Content,
                BLG_PublishedAt = request.BLG_PublishedAt,
                BLG_Status = request.BLG_Status,
                BLG_Title = request.BLG_Title,
                BLG_Slug = SlugHelper.GenerateSlug(request.BLG_Title, request.BLG_ID),
                CreatedBy = userInfo?.ACC_Username,
                CreatedDate = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                string? newImageFileName = null;

                if (request.BLG_Image != null)
                {
                    var extension = Path.GetExtension(request.BLG_Image.FileName);
                    newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "blog", newImageFileName);
                    request.BLG_Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                if (newImageFileName != null) blog.BLG_Image = newImageFileName;

                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BlogDTO request)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (id != request.BLG_ID)
            {
                return NotFound();
            }

            var blogExisting = await _context.Blogs.FindAsync(id);
            if (blogExisting == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogExisting.BLG_Status = request.BLG_Status;
                    blogExisting.BLG_Slug = SlugHelper.GenerateSlug(request.BLG_Title, request.BLG_ID);
                    blogExisting.BLG_ID = request.BLG_ID;
                    blogExisting.BLG_AuthorName = request.BLG_AuthorName;
                    blogExisting.BLG_Content = request.BLG_Content;
                    blogExisting.BLG_Title = request.BLG_Title;
                    blogExisting.BLG_PublishedAt = request.BLG_PublishedAt;
                    blogExisting.UpdatedDate = DateTime.Now;
                    blogExisting.CreatedBy = userInfo.ACC_Username;
                    blogExisting.UpdatedBy = userInfo.ACC_Username;
                    blogExisting.UpdatedDate = DateTime.Now;

                    if (request.BLG_Image != null)
                    {
                        if (!string.IsNullOrEmpty(blogExisting.BLG_Image))
                        {
                            var oldImagePath = Path.Combine(_hostEnv.WebRootPath, "images", "blog", blogExisting.BLG_Image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var extension = Path.GetExtension(request.BLG_Image.FileName);
                        var newImageFileName = $"{Guid.NewGuid().ToString()}{extension}";
                        var filePath = Path.Combine(_hostEnv.WebRootPath, "images", "blog", newImageFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.BLG_Image.CopyToAsync(stream);
                        }

                        blogExisting.BLG_Image = newImageFileName;
                    }

                    _context.Update(blogExisting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(request.BLG_ID))
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
            return View(blogExisting);
        }

        // GET: Admin/Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BLG_ID == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BLG_ID == id);
        }
    }
}
