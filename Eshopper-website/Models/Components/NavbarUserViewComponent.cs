using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
    public class NavbarUserViewComponent : ViewComponent
    {
        private readonly EShopperContext _context;

        public NavbarUserViewComponent(EShopperContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await _context.Menus.AsNoTracking()
                .Where(x => x.MEN_Status == MenuStatusEnum.User)
                .OrderBy(x => x.MEN_DisplayOrder)
                .ToListAsync();

            return View(menus);
        }
    }
}
