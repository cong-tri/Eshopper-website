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
    public class MenuController : Controller
    {
        private readonly EShopperContext _context;

        public MenuController(EShopperContext context)
        {
            _context = context;
        }

        // GET: Admin/Menu
        public async Task<IActionResult> Index()
        {
            var eShopperContext = _context.Menus.Include(m => m.Parent);
            return View(await eShopperContext.ToListAsync());
        }

        // GET: Admin/Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Parent)
                .FirstOrDefaultAsync(m => m.MEN_ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Admin/Menu/Create
        public IActionResult Create()
        {
            ViewData["PARENT_ID"] = new SelectList(_context.Menus, "MEN_ID", "MEN_Title");
            return View();
        }

        // POST: Admin/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["PARENT_ID"] = new SelectList(_context.Menus, "MEN_ID", "MEN_Title", menu.PARENT_ID);
            return View(menu);
        }

        // GET: Admin/Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["PARENT_ID"] = new SelectList(_context.Menus, "MEN_ID", "MEN_Title", menu.PARENT_ID);
            return View(menu);
        }

        // POST: Admin/Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Menu menu)
        {
            if (id != menu.MEN_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MEN_ID))
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
            ViewData["PARENT_ID"] = new SelectList(_context.Menus, "MEN_ID", "MEN_Title", menu.PARENT_ID);
            return View(menu);
        }

        // GET: Admin/Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Parent)
                .FirstOrDefaultAsync(m => m.MEN_ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Admin/Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MEN_ID == id);
        }
    }
}
