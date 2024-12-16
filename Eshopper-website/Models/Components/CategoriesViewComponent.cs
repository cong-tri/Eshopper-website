using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
    public class CategoriesViewComponent : ViewComponent
	{
		private readonly EShopperContext _context;

		public CategoriesViewComponent(EShopperContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Categories.AsNoTracking()
				.Where(x => x.CAT_Status.ToString() == "Active")
				.OrderBy(x => x.CAT_DisplayOrder).ToListAsync());
		}
	}
}
