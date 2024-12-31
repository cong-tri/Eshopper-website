using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.Components
{
    public class NavbarAdminViewComponent : ViewComponent
    {
        private readonly EShopperContext _context;

        public NavbarAdminViewComponent(EShopperContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(
                await _context.Menus.AsNoTracking()
                .Where(x => x.MEN_Status == MenuStatusEnum.Admin)
                .OrderBy(x => x.MEN_DisplayOrder)
                .ToListAsync()
            );
        }
    }
}
