using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Models.Components
{
    public class OrderConfirmedSendMailViewComponent : ViewComponent
    {
        private readonly EShopperContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        public OrderConfirmedSendMailViewComponent(EShopperContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInfo = HttpContext.Session.Get<Account>("userInfo");
            ViewData["Users"] = userInfo.ACC_Username;
             return View();
        }
    }
}
