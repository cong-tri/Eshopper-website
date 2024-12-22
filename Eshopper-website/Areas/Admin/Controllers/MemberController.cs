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
    public class MemberController : Controller
    {
        private readonly EShopperContext _context;

        public MemberController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Member
        public async Task<IActionResult> Index()
        {
            var eShopperContext = _context.Members.Include(m => m.Account).Include(m => m.AccountRole);
            return View(await eShopperContext.ToListAsync());
        }

        // GET: Admin/Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Account)
                .Include(m => m.AccountRole)
                .FirstOrDefaultAsync(m => m.MEM_ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Admin/Member/Create
        public IActionResult Create()
        {
            ViewData["ACC_ID"] = new SelectList(_context.Set<Account>(), "ACC_ID", "ACC_Username");
            ViewData["ACR_ID"] = new SelectList(_context.Set<AccountRole>(), "ACR_ID", "ACR_Name");
            return View();
        }

        // POST: Admin/Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ACC_ID"] = new SelectList(_context.Set<Account>(), "ACC_ID", "ACC_Username", member.ACC_ID);
            ViewData["ACR_ID"] = new SelectList(_context.Set<AccountRole>(), "ACR_ID", "ACR_Name", member.ACR_ID);
            return View(member);
        }

        // GET: Admin/Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["ACC_ID"] = new SelectList(_context.Set<Account>(), "ACC_ID", "ACC_DisplayName", member.ACC_ID);
            ViewData["ACR_ID"] = new SelectList(_context.Set<AccountRole>(), "ACR_ID", "ACR_Name", member.ACR_ID);
            return View(member);
        }

        // POST: Admin/Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Member member)
        {
            if (id != member.MEM_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MEM_ID))
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
            ViewData["ACC_ID"] = new SelectList(_context.Set<Account>(), "ACC_ID", "ACC_Username", member.ACC_ID);
            ViewData["ACR_ID"] = new SelectList(_context.Set<AccountRole>(), "ACR_ID", "ACR_Name", member.ACR_ID);
            return View(member);
        }

        // GET: Admin/Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Account)
                .Include(m => m.AccountRole)
                .FirstOrDefaultAsync(m => m.MEM_ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Admin/Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MEM_ID == id);
        }
    }
}
