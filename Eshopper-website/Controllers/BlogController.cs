using Microsoft.AspNetCore.Mvc;
using Eshopper_website.Models.DataContext;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;

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
			return View(blogs);
		}

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
	}
}
