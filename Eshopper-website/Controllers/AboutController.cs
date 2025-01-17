using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
