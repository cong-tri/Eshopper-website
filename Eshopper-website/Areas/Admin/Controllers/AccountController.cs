using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Areas.Admin.Repository;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly EShopperContext _context;

        public AccountController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }
        public async Task<IActionResult> NewPass(string returnUrl)
        {
            return View(await _context.Accounts.ToListAsync());
        }
        //[HttpPost]
        //public async Task<IActionResult> SendMailFogotPass(Account account)
        //{
        //    var checkMail = await account.ACC_Username.FirstOrDefaultAsync(c => c.ACC_Email == account.ACC_Email);
        //    if (checkMail == null)
        //    {
        //        TempData["error"] = "Email not found!";
        //        return RedirectToAction("ForgetPass", "Account");
        //    }
        //    else
        //    {
        //        string token = Guid.NewGuid().ToString();
        //        //Update token to user
        //        checkMail.Token = token;
        //        _context.Update(checkMail);
        //        await _context.SaveChangesAsync();
        //        var receiver = checkMail.Email;
        //        var subject = "Change password for user" + checkMail.Email;
        //        var message = "Click on click here to change your password"
        //            + "<a href= ' " + $" {Request.Scheme}://{Request.Host}/Account/NewPass" + $"?email=" + checkMail.Email + "&token" + token + "'>";

        //        await EmailSender.SendMailAsync(receiver, subject, message);
        //    }
        //    TempData["Sucess"] = "An email has been sent to your registered email address with password reset intructions.";
        //    return RedirectToAction("ForgetPass", "Account");
        //}
        public async Task<IActionResult> FogetPass(string returnUrl)
        {
            return View(await _context.Accounts.ToListAsync());
        }
        // GET: Admin/Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.ACC_ID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Admin/Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Admin/Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Account account)
        {
            if (id != account.ACC_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.ACC_ID))
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
            return View(account);
        }

        // GET: Admin/Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.ACC_ID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.ACC_ID == id);
        }
    }
}
