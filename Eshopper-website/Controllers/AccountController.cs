using Eshopper_website.Areas.Admin.DTOs.request;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eshopper_website.Controllers
{
    public class AccountController : Controller
    {
        private readonly EShopperContext _context;
        public AccountController(EShopperContext context, IOptions<Appsettings> options)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }
            var profile = await _context.Members.AsNoTracking()
                .Include(x => x.Account)
                .Where(x => x.ACC_ID == userInfo.ACC_ID).FirstOrDefaultAsync();

            if (profile == null) {
                TempData["error"] = "Profile not found!";
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

            var historyOrder = _context.Orders.AsNoTracking()
                .Include(x => x.Member)
                .Where(x => x.MEM_ID == profile.MEM_ID).FirstOrDefaultAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProfileDTO request)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

            if (userInfo.ACC_ID != request.ACC_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(m => m.ACC_ID == request.ACC_ID);

                var member = _context.Members.FirstOrDefaultAsync(x => x.MEM_ID == request.MEM_ID);

                if (account == null || member == null) 
                {
                    ViewData["Message"] = "Account or member not found";
                    return View(request);
                }
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(request.MEM_ID) || !AccountExists(request.ACC_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.ACC_ID == id);
        }
        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MEM_ID == id);
        }
    }
}
