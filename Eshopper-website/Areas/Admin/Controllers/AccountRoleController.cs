using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountRoleController : Controller
    {
        private readonly EShopperContext _context;

        public AccountRoleController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/AccountRole
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountRole.ToListAsync());
        }

        // GET: Admin/AccountRole/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountRole = await _context.AccountRole
                .FirstOrDefaultAsync(m => m.ACR_ID == id);
            if (accountRole == null)
            {
                return NotFound();
            }

            return View(accountRole);
        }

        // GET: Admin/AccountRole/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AccountRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AccountRole accountRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountRole);
        }

        // GET: Admin/AccountRole/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountRole = await _context.AccountRole.FindAsync(id);
            if (accountRole == null)
            {
                return NotFound();
            }
            return View(accountRole);
        }

        // POST: Admin/AccountRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] AccountRole accountRole)
        {
            if (id != accountRole.ACR_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountRoleExists(accountRole.ACR_ID))
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
            return View(accountRole);
        }

        // GET: Admin/AccountRole/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountRole = await _context.AccountRole
                .FirstOrDefaultAsync(m => m.ACR_ID == id);
            if (accountRole == null)
            {
                return NotFound();
            }

            return View(accountRole);
        }

        // POST: Admin/AccountRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountRole = await _context.AccountRole.FindAsync(id);
            if (accountRole != null)
            {
                _context.AccountRole.Remove(accountRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountRoleExists(int id)
        {
            return _context.AccountRole.Any(e => e.ACR_ID == id);
        }
    }
}
