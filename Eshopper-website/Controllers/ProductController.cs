using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Controllers
{
	public class ProductController : Controller
	{
		private readonly EShopperContext _context;

		public ProductController(EShopperContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewData["products"] = _context.Products.Include(x => x.Category).Include(x => x.Brand).ToList();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _context.Products
				.Include(x => x.Category)
				.Include(x => x.Brand)
				.Where(p => p.PRO_Name.Contains(searchTerm) || p.PRO_Description.Contains(searchTerm))
				.ToListAsync();

			ViewBag.Keyword = searchTerm;
			return View(products);
		}

		public IActionResult Details(string? Slug)
		{
			if (String.IsNullOrEmpty(Slug))
			{
                ViewData["products"] = _context.Products.Include(x => x.Category).Include(x => x.Brand).ToList();
                return View("Index");
			}

			var productsById = _context.Products
				.Include(p => p.Brand)
				.Include(p => p.Category)
				.Include(p => p.Ratings)
				.FirstOrDefault(p => p.PRO_Slug == Slug);

			if (productsById == null)
			{
				return NotFound();
			}

			// Get ratings for this product
			var ratings = _context.Ratings
				.Where(r => r.PRO_ID == productsById.PRO_ID)
				.OrderByDescending(r => r.CreatedDate)
				.ToList();

			var relantedProduct = _context.Products
				.Where(p => p.CAT_ID == productsById.CAT_ID && p.PRO_ID != productsById.PRO_ID)
				.Take(4)
				.ToList();

			var viewModel = new ProductDetailsView
			{
				ProductDetail = productsById,
				Ratings = ratings
			};

			ViewData["relantedProduct"] = relantedProduct;

			return View(viewModel);
		}

		public async Task<IActionResult> CommentProduct(Rating rating)
		{
			if (ModelState.IsValid)
			{
				var ratingEntity = new Rating
				{
					PRO_ID = rating.PRO_ID,
					RAT_Name = rating.RAT_Name,
					RAT_Email = rating.RAT_Email,
					RAT_Comment = rating.RAT_Comment,
					RAT_Star = rating.RAT_Star
				};

				_context.Ratings.Add(ratingEntity);
				await _context.SaveChangesAsync();
				TempData["success"] = "Thêm đánh giá thành công";
				return Redirect(Request.Headers["Referer"]);
			}
			else
			{
				TempData["error"] = "Model co 1 vai loi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return RedirectToAction("Detail", new { id = rating.PRO_ID });
			}
		}
	}
}

