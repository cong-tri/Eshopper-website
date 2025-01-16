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

            ViewData["OrderHistory"] = await _context.Orders.AsNoTracking()
                .Include(x => x.Member)
                .Where(x => x.MEM_ID == profile.MEM_ID).ToListAsync();

            var profileDTO = new ProfileDTO()
            {
                ACC_ID = userInfo.ACC_ID,
                MEM_ID = userInfo.MEM_ID,
                ACC_DisplayName = userInfo.ACC_DisplayName,
                MEM_Email = profile.MEM_Email,
                MEM_Phone = profile.MEM_Phone,
                MEM_Address = profile.MEM_Address,
                MEM_FirstName = profile.MEM_FirstName,
                MEM_LastName = profile.MEM_LastName,
                MEM_Gender = profile.MEM_Gender,
            };

            return View(profileDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProfileDTO request)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (userInfo?.ACC_ID != request.ACC_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var account = await _context.Accounts.FindAsync(request.ACC_ID);

                var member = await _context.Members.FindAsync(request.MEM_ID);

                if (account == null || member == null) 
                {
                    ViewData["Message"] = "Account or member not found";
                    return View(request);
                }
                try
                {
                    member.MEM_Gender = request.MEM_Gender;
                    member.MEM_FirstName = request.MEM_FirstName;
                    member.MEM_LastName = request.MEM_LastName;
                    member.MEM_Email = request.MEM_Email;
                    member.MEM_Phone = request.MEM_Phone;
                    account.ACC_Phone = request.MEM_Phone;
                    account.ACC_Email = request.MEM_Email;
                    account.ACC_DisplayName = request.ACC_DisplayName ?? "";
                    member.MEM_Address = request.MEM_Address;

                    _context.Members.Update(member);
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();

                    var user = new UserInfo(account, member.ACR_ID, member.MEM_ID);
                    HttpContext.Session.Set<UserInfo>("userInfo", user);

                    TempData["success"] = "Update profile successful.";
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(request.MEM_ID) || !AccountExists(request.ACC_ID))
                    {
                        ViewData["Message"] = "Account and member not found.";
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
