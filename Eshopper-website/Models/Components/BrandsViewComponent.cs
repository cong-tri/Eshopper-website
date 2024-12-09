using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
    public class BrandsViewComponent : ViewComponent
	{
		private readonly EShopperContext _context;

		public BrandsViewComponent(EShopperContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Brands.AsNoTracking().OrderBy(x => x.BRA_DisplayOrder).ToListAsync());
		}
	}
}
