using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
	[ViewComponent(Name = "Footer")]
	public class FooterViewComponent : ViewComponent
	{
		private readonly EShopperContext _context;

		public FooterViewComponent(EShopperContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _context.Contacts.AsNoTracking().FirstOrDefaultAsync());
		}
	}
}
