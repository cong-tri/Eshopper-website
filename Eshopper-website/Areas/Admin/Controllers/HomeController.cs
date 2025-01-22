using Eshopper_website.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController(EShopperContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.CountProduct = await context.Products.CountAsync();
            ViewBag.CountOrder = await context.Orders.CountAsync();
            ViewBag.CountCategory = await context.Categories.CountAsync();
            ViewBag.CountUser = await context.Accounts.CountAsync();

            ViewData["orders"] = await context.Orders.AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

                //.Include(x => x.OrderDetails)!
                //.ThenInclude(x => x.Product)

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChartData()
        {
            try
            {
                var sevenDaysAgo = DateTime.Now.AddDays(-7);
                var query = await context.Statisticals
                    .Where(s => s.CreatedDate >= sevenDaysAgo)
                    .OrderBy(s => s.CreatedDate)
                    .Select(s => new
                    {
                        date = s.CreatedDate.ToString("dd/MM/yyyy"),
                        sold = s.STA_Sold.GetValueOrDefault(),
                        quantity = s.STA_Quantity.GetValueOrDefault(),
                        revenue = s.STA_Revenue.GetValueOrDefault(),
                        profit = s.STA_Profit.GetValueOrDefault()
                    })
                    .ToListAsync();

                if (!query.Any())
                {
                    return GetDefaultData();
                }

                return Json(query);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching statistical data", details = ex.Message });
            }
        }

        private JsonResult GetDefaultData()
        {
            var defaultData = Enumerable.Range(0, 7)
                .Select(i => new
                {
                    date = DateTime.Now.AddDays(-i).ToString("dd/MM/yyyy"),
                    sold = 0,
                    quantity = 0,
                    revenue = 0m,
                    profit = 0m
                })
                .OrderBy(x => DateTime.ParseExact(x.date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .ToList();

            return Json(defaultData);
        }
    }
}
