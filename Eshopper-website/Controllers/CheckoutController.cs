﻿using Eshopper_website.Areas.Admin.Repository;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Models.ViewModels;
using Eshopper_website.Utils.Enum.Order;
using Eshopper_website.Utils.Extension;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshopper_website.Controllers
{
	public class CheckoutController : Controller
	{
        private readonly EShopperContext _context;
        private readonly  IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnv;
        public CheckoutController(IEmailSender emailSender ,EShopperContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _emailSender = emailSender;
            _hostEnv = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Checkout()
        {
            var userInfo = HttpContext.Session.Get<Account>("userInfo");
            if (userInfo == null)
            {
                return RedirectToAction("Login", "User", new { Area = "Admin" });
            }

            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItemView cartItemView = new()
            {
                GrandTotal = cartItems.Sum(x => x.PRO_Quantity * x.PRO_Price)
            };

            var ordercode = Guid.NewGuid().ToString();
            var orderItem = new Order()
            {
                MEM_ID = 1,
                ORD_OrderCode = ordercode,
                ORD_Description = "This order is created by admin in order to testing.",
                ORD_Status = OrderStatusEnum.Pending,
                ORD_PaymentMethod = OrderPaymentMethodEnum.Cash,
                ORD_ShippingCost = 100,
                CreatedBy = "admin",
                ORD_TotalPrice = cartItemView.GrandTotal,
                CreatedDate = DateTime.Now,
            };

            _context.Add(orderItem);
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
                    CreatedBy = "admin"
                };

                _context.Add(orderDetails);
                await _context.SaveChangesAsync();

            }
            HttpContext.Session.Remove("Cart");

            //string Body = await HtmlRenderer
            string receiver = userInfo.ACC_Email;
            string subject = "ORDER HAVE BEEN CREATED SUCCESSFULLY!";
            string message = "Your Order have been created successfully. Please waiting for shop owner confirmed!";
            await _emailSender.SendEmailAsync(receiver, subject, EmailTemplates.GetOrderConfirmationEmail(orderItem));

            TempData["success"] = "Order has been created successfully! Please wait for the order has been confirmed!";
            return RedirectToAction("Index", "Cart");
        }
	}
}
