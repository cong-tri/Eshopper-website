using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VnInfoController : Controller
    {
        private readonly EShopperContext _context;
        public VnInfoController(EShopperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var momoInfos = _context.VnInfos
                .AsNoTracking()
                .ToList();

            return View(momoInfos);
        }
    }
}
