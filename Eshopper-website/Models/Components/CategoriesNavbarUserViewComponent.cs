using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
	public class CategoriesNavbarUserViewComponent: ViewComponent
	{
		private readonly EShopperContext _context;

		public CategoriesNavbarUserViewComponent(EShopperContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Categories.AsNoTracking()
				.Where(x => x.CAT_Status.Equals(CategoryStatusEnum.Active))
				.OrderBy(x => x.CAT_DisplayOrder).ToListAsync());
		}
	}
}
