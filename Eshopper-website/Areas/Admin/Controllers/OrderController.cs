using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Enum.Order;
using Eshopper_website.Services.GHN;
using Eshopper_website.Models.GHN;
using Eshopper_website.Models.ViewModels;
using Eshopper_website.Models;
using NuGet.Protocol;
using Eshopper_website.Models.GHN.Response;
using Microsoft.Extensions.Options;
using Eshopper_website.Areas.Admin.Repository;
using NuGet.Protocol.Plugins;
using Eshopper_website.Utils.Extension;


namespace Eshopper_website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly EShopperContext _context;
        private readonly IGHNService _ghnService;
        private readonly IOptions<GHN_Setting> _options;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            EShopperContext context, 
            IGHNService ghnService,
            IOptions<GHN_Setting> options, 
            IEmailSender emailSender, 
            ILogger<OrderController> logger)
        {
            _context = context;
            _ghnService = ghnService;
            _options = options;
            _emailSender = emailSender;
            _logger = logger;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            try 
            {
                var orders = await _context.Orders
                    .Include(o => o.Member)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();
                
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                TempData["error"] = "Having error when load data order.";
                return View(new List<Order>());
            }
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Order details requested with null ID");
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Member)
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(m => m.ORD_ID == id);

            ViewData["OrderDetails"] = await _context.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(x => x.ORD_ID == id)
                .ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            ViewData["orderStatus"] = Enum.GetValues(typeof(OrderStatusEnum))
                .Cast<OrderStatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int id, OrderStatusEnum statusEnum)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

            if (userInfo == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.ORD_ID == id);

            if (order == null)
            {
                _logger.LogWarning("Order not found with ID: {OrderId}", id);
                return NotFound();
            }

            order.ORD_Status = statusEnum;

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                var newOrder = await _context.Orders.FirstOrDefaultAsync(o => o.ORD_ID == order.ORD_ID);

                //await _emailSender.SendEmailAsync(
                //    userInfo?.ACC_Email!,
                //    "YOUR ORDER STATUS HAVE BEEN UPDATED",
                //    $@"
                //        Your order: {newOrder?.ORD_OrderCode} have been {newOrder?.ORD_Status}. </br>
                //    "
                //    );

                return Ok(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for ID: {OrderId}", id);
                TempData["error"] = "Có lỗi xảy ra khi tải thông tin đơn hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ORD_ID == id);
        }

        [HttpGet]
        public IActionResult CreateGHNOrder()
        {
            ViewData["OrderCodes"] = new SelectList(
                _context.Orders.AsNoTracking()
                .Where(x => x.ORD_Status == OrderStatusEnum.Confirmed
                && x.OrderDetails!.Count != 0 && x.ORD_IsGHN == OrderIsGHNEnum.Inactive
                ), 
                "ORD_OrderCode", "ORD_OrderCode"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGHNOrder(GHNOrderView viewModel)
        {
			var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");

			if (ModelState.IsValid)
            {
                var orderExisting = await _context.Orders.AsNoTracking()
                    .Include(x => x.OrderDetails!)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x!.Category)
                    .FirstOrDefaultAsync(x => x.ORD_OrderCode == viewModel.ClientOrderCode);

                if (orderExisting == null) 
                {
                    return NotFound();
                }

                try
                {
                    var order = new GHN_Order
                    {
                        FromName = "EShopper Electronics",
                        FromPhone = "0326034561",
                        FromAddress = "999 Lê Đức Thọ, Phường 16, Quận Gò Vấp, Hồ Chí Minh, VietNam",
                        FromDistrictName = "Quận Gò Vấp",
                        FromProviceName = "HCM",
                        FromWardName = "Phường 16",
                        ToName = viewModel.ToName,
                        ToPhone = viewModel.ToPhone,
                        ToAddress = viewModel.ToAddress,
                        ToWardCode = viewModel.ToWardCode,
                        ToDistrictId = viewModel.ToDistrictId,
                        ClientOrderCode = orderExisting.ORD_OrderCode,
                        CodAmount = ((int)orderExisting.ORD_TotalPrice),
                        Weight = viewModel.Weight,
                        Length = viewModel.Length,
                        Width = viewModel.Width,
                        Height = viewModel.Height,
                        PickShift = [2],
                        Content = orderExisting.ORD_Description,
                        Items = orderExisting.OrderDetails!.Select(i => new GHN_OrderItem
                        {
                            Name = i.Product!.PRO_Name,
                            Quantity = i.ORDE_Quantity,
                            Price = ((int)i.ORDE_Price),
                            Code = i.Product.PRO_Slug,
                            Level1 = i.Product?.Category?.CAT_Name
                        }).ToList()
                    };

                    var response = await _ghnService.CreateOrderAsync(order);

                    if (response.code != 200 && response.data == null)
                    {
                        return BadRequest(new { error = response.message });
                    }

                    //orderExisting.ORD_IsGHN = OrderIsGHNEnum.Active;
                    //_context.Orders.Update(orderExisting);
                    //await _context.SaveChangesAsync();

					var newOrderGHN = new OrderGHN()
					{
						OrderCode = response.data.order_code,
						SortCode = response.data.sort_code,
						TotalFee = (int)response.data.total_fee,
						TransType = response.data.trans_type,
						ExpectedDeliveryTime = response.data.expected_delivery_time,
						CreatedBy = userInfo.ACC_DisplayName,
						CreatedDate = DateTime.Now,
					};

					_context.OrderGHNs.Add(newOrderGHN);
					await _context.SaveChangesAsync();

                    TempData["success"] = "Order GHN created successfully!";
					return Ok(response);
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to create order: " + ex.Message;
                    return View(viewModel);
                }
            }
            else
            {
                TempData["error"] = "Have error when create order ghn!";
                return View(viewModel);
            }
        }
    }
}
