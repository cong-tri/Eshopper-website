﻿using Eshopper_website.Areas.Admin.Repository;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.ViewModels;
using Eshopper_website.Models.VNPay;
using Eshopper_website.Services.Momo;
using Eshopper_website.Services.VNPay;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Order;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Eshopper_website.Controllers
{
	public class CheckoutController : Controller
	{
        private readonly IVnPayService _vnPayService;
        private readonly EShopperContext _context;
        private readonly IEmailSender _emailSender;
		private readonly IMomoService _momoService;


        public CheckoutController(
            IEmailSender emailSender, EShopperContext context, 
            IVnPayService vnPayService, IMomoService momoService)
        {
            _context = context;
            _emailSender = emailSender;
            _vnPayService = vnPayService;
            _momoService = momoService;
        }
        public IActionResult Index()
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null) 
            {
                TempData["error"] = "Please login before checkout!";
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;

            if (shippingPriceCookie != null)
            {
                var shippingPriceJson = shippingPriceCookie;
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
            }

            ViewData["coupon_code"] = Request.Cookies["CouponTitle"];

            ViewData["shippingPrice"] = shippingPrice;

            ViewData["Payment"] = new SelectList(
                _context.Payments.Where(x => x.PAY_Status == PaymentStatusEnum.Active), "PAY_ID", "PAY_Name"
            );

            ViewData["listCartItems"] = cartItems;

            var checkoutView = new CheckoutView()
            {
                MEM_ID = userInfo.MEM_ID,
                FullName = $"{userInfo.ACC_DisplayName}",
                Phone = userInfo.ACC_Phone!,
                Email = userInfo.ACC_Email!,
                ShippingPrice = shippingPrice,
                CartItems = cartItems,
            };

            return View(checkoutView);
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] CheckoutView request)
        {
            var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];
            decimal grandTotal = 0;
            foreach (var item in cartItems)
            {
                grandTotal += item.PRO_Quantity * item.PRO_Price;
            }

            CartItemView cartItemView = new()
            {
                GrandTotal = grandTotal
            };

            var ordercode = Guid.NewGuid().ToString();

            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;

            if (!string.IsNullOrEmpty(shippingPriceCookie))
            {
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceCookie);
            }
            //Nhận coupon code
            var CouponCode = Request.Cookies["CouponTitle"];

            var orderItem = new Order()
            {
                MEM_ID = request.MEM_ID,
                ORD_OrderCode = ordercode,
                ORD_Description = $"Order had been ordered by {userInfo.ACC_Username}.",
                ORD_Status = OrderStatusEnum.Pending,
                ORD_PaymentMethod = request.PaymentMethod,
                ORD_ShippingCost = request.ShippingPrice,
                ORD_CouponCode = CouponCode,
                ORD_TotalPrice = cartItemView.GrandTotal + shippingPrice,
                ORD_ShipAddress = request.ToAddress,
                ORD_IsGHN = OrderIsGHNEnum.Inactive,
                CreatedBy = userInfo.ACC_Username,
                CreatedDate = DateTime.Now,
            };

            _context.Orders.Add(orderItem);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
			{
				var orderDetails = new OrderDetail()
				{
					ORD_ID = orderItem.ORD_ID,
					PRO_ID = item.PRO_ID,
					ORDE_Price = item.PRO_Price,
					ORDE_Quantity = item.PRO_Quantity,
					CreatedDate = DateTime.Now,
					CreatedBy = userInfo.ACC_Username
				};

				var product = await _context.Products.Where(p => p.PRO_ID == item.PRO_ID).FirstOrDefaultAsync();

				product!.PRO_Quantity -= item.PRO_Quantity;
				product.PRO_Sold += item.PRO_Quantity;

				if (product.PRO_Quantity == 0)
				{
					product.PRO_Status = ProductStatusEnum.OutOfStock;
				}

				if (product.PRO_Quantity < 20)
				{
					product.PRO_Status = ProductStatusEnum.LowStock;
				}

				_context.Products.Update(product);
				_context.OrderDetails.Add(orderDetails);
				await _context.SaveChangesAsync();
			}

			var orderSend = await _context.Orders.AsNoTracking()
				.Include(x => x.Member)
						 .Include(x => x.OrderDetails!)
						 .ThenInclude(x => x.Product)
						 .FirstOrDefaultAsync(x => x.ORD_ID == orderItem.ORD_ID);

            if (orderSend == null)
            {
                return NotFound();
            }

            switch (request.PaymentMethod)
            {
                case 1:
                    orderItem.ORD_Status = OrderStatusEnum.Processing;
                    _context.Orders.Update(orderItem);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove("Cart");
                    break;

                case 2:
                    orderItem.ORD_Status = OrderStatusEnum.Processing;
                    _context.Orders.Update(orderItem);
                    await _context.SaveChangesAsync();

                    var amount = cartItemView.GrandTotal + shippingPrice;

					var orderInfo = new OrderInfo()
                    {
                        Amount = ((double)orderSend!.ORD_TotalPrice),
                        FullName = request.FullName,
                        OrderId = orderSend.ORD_OrderCode,
                        OrderInformation = "Payment with Momo at EShopper Electronics."
					};
					var response = await _momoService.CreatePaymentAsync(orderInfo);
					return Redirect(response.PayUrl!);

                case 3:
                    orderItem.ORD_Status = OrderStatusEnum.Processing;
                    _context.Orders.Update(orderItem);
                    await _context.SaveChangesAsync();

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    var orderVNPay = new PaymentInformationModel()
                    {
                        OrderType = "other",
                        Amount = ((double)orderSend!.ORD_TotalPrice),
                        OrderDescription = orderSend.ORD_Description,
                        Name = request.FullName,
                        Email = request.Email,
                        PhoneNumber = request.Phone,
                        ReturnUrl = $"{baseUrl}/Checkout/PaymentCallbackVnpay"
                    };
                    var url = _vnPayService.CreatePaymentUrl(orderVNPay, HttpContext);
                    return Redirect(url);


                default:
                    break;
			}

			//HttpContext.Session.Remove("Cart");

            string receiver = userInfo.ACC_Email!;
            string subject = "ORDER HAVE BEEN CREATED SUCCESSFULLY!";

            //var responseSendMail = await _emailSender.SendEmailAsync(receiver, subject, EmailTemplates.GetOrderConfirmationEmail(orderSend));

            //if (responseSendMail.Code == 404)
            //{
            //    TempData["error"] = "Cannot create order. Have error when create order!";
            //    return RedirectToAction("Index", "Checkout");
            //}

            await _emailSender.SendEmailAsync(receiver, subject, EmailTemplates.GetOrderConfirmationEmail(orderSend));

            TempData["success"] = "Order has been created successfully! Please wait for the order has been confirmed!";
            return RedirectToAction("Success", "Checkout");
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCallbackVnpay()
		{
			var response = await Task.FromResult(_vnPayService.PaymentExecute(Request.Query));

            if (response.Success == true)
            {
                var newVnInfo = new VnInfo()
                {
                    OrderId = response.OrderId,
                    TransactionId = response.TransactionId,
                    PaymentId = response.PaymentId,
                    PaymentMethod = response.PaymentMethod,
                    OrderDescription = response.OrderDescription,
                    Amount = response.Amount,
                    CreatedDate = DateTime.Now,
                };

                _context.VnInfos.Add(newVnInfo);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");

                TempData["success"] = "Payment with vnpay successfully.";
                //return RedirectToAction("Success", "Checkout");

                return Redirect("/Checkout/Success");
            }
            else
            {
                TempData["error"] = "Have failed when payment with vnpay.";
                return RedirectToAction("Index", "Checkout");
            }

			return Json(response);
		}

		[HttpGet]
        public async Task<IActionResult> PaymentCallBack(MomoInfo model)
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;

            if (requestQuery != null && requestQuery["message"] == "Success")
            {
                var newMomo = new MomoInfo()
                {
                    OrderId = requestQuery["orderId"],
                    OrderInfo = requestQuery["orderInfo"],
                    MOMO_FullName = "Nguyen Van A",
                    MOMO_Amount = decimal.Parse(requestQuery["amount"]),
                    MOMO_DatePaid = DateTime.Now,
                    CreatedDate = DateTime.Now
                };

                _context.Momos.Add(newMomo);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");

                TempData["success"] = "Payment with momo successfully.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Cancel payment transaction momo.";
                return RedirectToAction("Index", "Checkout");
            }
        }
    }
}
