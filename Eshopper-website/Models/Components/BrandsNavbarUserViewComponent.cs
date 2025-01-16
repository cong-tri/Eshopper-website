using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
	public class BrandsNavbarUserViewComponent : ViewComponent
	{
		private readonly EShopperContext _context;

		public BrandsNavbarUserViewComponent(EShopperContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Brands.AsNoTracking()
				.Where(x => x.BRA_Status.Equals(BrandStatusEnum.Active))
				.OrderBy(x => x.BRA_DisplayOrder).ToListAsync());
		}
	}
}
