using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly EShopperContext _context;
        public UserController(EShopperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            // Check cookies
            var login = Request.Cookies.Get<LoginDTO>("UserCredential");
            if (login != null)
            {
                var result = _context.Accounts.AsNoTracking()
                                .FirstOrDefault(x => x.ACC_Username == login.UserName &&
                                    x.ACC_Password == login.Password);
                if (result != null)
                {
                    // Set Session
                    HttpContext.Session.Set<Account>("userInfo", result);
                    // Redirect to Dashboard
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            var result = await _context.Accounts.AsNoTracking()
                                .FirstOrDefaultAsync(x => x.ACC_Username == login.UserName &&
                                    x.ACC_Password == login.Password);
            if (result != null && result.ACC_Status == AccountStatusEnum.Active)
            {
                if (login.RememberMe)
                {
                    Response.Cookies.Append<LoginDTO>("UserCredential", login, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true, // Accessible only by the server
                        IsEssential = true // Required for GDPR compliance
                    });
                }

                // Set Session
                HttpContext.Session.Set<Account>("userInfo", result);
                // Redirect to Dashboard
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Message"] = "Wrong username or password!";
            }
            return View();
        }

    }
}
