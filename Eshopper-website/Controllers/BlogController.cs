using Microsoft.AspNetCore.Mvc;
using Eshopper_website.Models.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
	public class BlogController : Controller
	{
		private readonly EShopperContext _context;

		public BlogController(EShopperContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var blogs = await _context.Blogs
				.OrderByDescending(b => b.BLG_PublishedAt)
				.ToListAsync();
			ViewData["Blogs"] = blogs;
			return View();
		}

		public async Task<IActionResult> Details(string? slug)
		{
			if (String.IsNullOrEmpty(slug))
			{
                var blogs = await _context.Blogs
                .OrderByDescending(b => b.BLG_PublishedAt)
                .ToListAsync();

                ViewData["Blogs"] = blogs;

				return View("Index");
            }

			var blog = await _context.Blogs
				.FirstOrDefaultAsync(m => m.BLG_Slug == slug);

			if (blog == null)
			{
				return NotFound();
			}

			return View(blog);
		}
	}
}
