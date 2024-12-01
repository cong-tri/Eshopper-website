﻿using Microsoft.AspNetCore.Mvc;
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
			return View(await _context.Categories.AsNoTracking().OrderBy(x => x.CAT_DisplayOrder).ToListAsync());
		}
	}
}
