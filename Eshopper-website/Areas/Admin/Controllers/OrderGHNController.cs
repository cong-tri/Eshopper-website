using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.GHN;
using Eshopper_website.Services.GHN;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eshopper_website.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderGHNController : Controller
	{
		private readonly EShopperContext _context;
		public OrderGHNController(EShopperContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var orderGHN = _context.OrderGHNs.AsNoTracking()
				.ToList();
			return View(orderGHN);
		}
	}
}
