using Eshopper_website.Models;
using Eshopper_website.Models.VNPay;
using Eshopper_website.Services.Momo;
using Eshopper_website.Services.VNPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Eshopper_website.Utils.Enum.Order;
using Newtonsoft.Json;
using Eshopper_website.Utils.Enum;

namespace Eshopper_website.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly IMomoService _momoService;
        public readonly EShopperContext _context;

        public PaymentController(IVnPayService vnPayService, IMomoService momoService, EShopperContext context)
        {    
            _context = context;
            _vnPayService = vnPayService;
            _momoService = momoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfo model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl!);
        }

        [HttpGet]
        public IActionResult PaymentCallBackMomo()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrlVnpay([FromForm] PaymentInformationModel model)
        {
            try
            {

                //// Validate amount
                //if (model.Amount <= 0)
                //{
                //    TempData["ErrorMessage"] = "Số tiền phải lớn hơn 0";
                //    return RedirectToAction("Index", "Cart");
                //}

                //if (model.Amount > 100000000)
                //{
                //    TempData["ErrorMessage"] = "Số tiền không được vượt quá 100,000,000 VNĐ";
                //    return RedirectToAction("Index", "Cart");
                //}

                //var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
                //if (userInfo == null)
                //{
                //    TempData["ErrorMessage"] = "Vui lòng đăng nhập để thanh toán";
                //    return RedirectToAction("Login", "Account");
                //}

                //// Store original amount for database
                //decimal originalAmount = model.Amount;
                //Console.WriteLine($"Original Amount: {originalAmount}");

                //// Convert to integer amount for VNPay (no decimals)
                //var intAmount = (long)Math.Floor(originalAmount);
                //Console.WriteLine($"Integer Amount: {intAmount}");

                //var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                //decimal shippingPrice = 0;

                //if (!string.IsNullOrEmpty(shippingPriceCookie))
                //{
                //    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceCookie);
                //}

                //// Create order
                //var order = new Order()
                //{
                //    MEM_ID = userInfo.MEM_ID,
                //    ORD_OrderCode = Guid.NewGuid().ToString("N"),
                //    ORD_Description = $"Order had been ordered by {userInfo.ACC_Username}.",
                //    ORD_Status = OrderStatusEnum.WaitingForPayment,
                //    ORD_PaymentMethod = OrderPaymentMethodEnum.VNPay,
                //    ORD_TotalPrice = originalAmount,
                //    ORD_ShippingCost = shippingPrice,
                //    CreatedDate = DateTime.Now,
                //    CreatedBy = userInfo.ACC_Username
                //};

                //_context.Orders.Add(order);
                //await _context.SaveChangesAsync();

                //Console.WriteLine($"Created Order ID: {order.ORD_ID}");
                //Console.WriteLine($"Order Code: {order.ORD_OrderCode}");

                //// Add order details
                //List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? [];
                //foreach (var item in cartItems)
                //{
                //    var orderDetail = new OrderDetail
                //    {
                //        ORD_ID = order.ORD_ID,
                //        PRO_ID = item.PRO_ID,
                //        ORDE_Quantity = item.PRO_Quantity,
                //        ORDE_Price = item.PRO_Price,
                //        CreatedDate = DateTime.Now,
                //        CreatedBy = userInfo.ACC_Username
                //    };

                //    var product = await _context.Products.Where(p => p.PRO_ID == item.PRO_ID).FirstOrDefaultAsync();

                //    product!.PRO_Quantity -= item.PRO_Quantity;
                //    product.PRO_Sold += item.PRO_Quantity;

                //    if (product.PRO_Quantity == 0)
                //    {
                //        product.PRO_Status = ProductStatusEnum.OutOfStock;
                //    }

                //    if (product.PRO_Quantity < 20)
                //    {
                //        product.PRO_Status = ProductStatusEnum.LowStock;
                //    }

                //    _context.Products.Update(product);
                //    _context.OrderDetails.Add(orderDetail);
                //    await _context.SaveChangesAsync();
                //}

                //// Update payment model
                ////model. = order.ORD_OrderCode;
                //model.Amount = intAmount;
                //model.OrderDescription = $"Thanh toán đơn hàng {order.ORD_OrderCode}";

                //Console.WriteLine($"Payment Model - Amount: {model.Amount}");
                ////Console.WriteLine($"Payment Model - Order ID: {model.OrderId}");
                //Console.WriteLine($"Payment Model - Description: {model.OrderDescription}");

                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                // Clear cart and cookies
                HttpContext.Session.Remove("Cart");
                Response.Cookies.Delete("CouponTitle");
                Response.Cookies.Delete("ShippingPrice");

                return Redirect(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatePaymentUrlVnpay: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = $"Lỗi khi xử lý thanh toán: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            try
            {
                Console.WriteLine("=== Starting PaymentCallback ===");

                if (!Request.Query.ContainsKey("vnp_SecureHash"))
                {
                    Console.WriteLine("Missing secure hash");
                    TempData["ErrorMessage"] = "Yêu cầu không hợp lệ";
                    return RedirectToAction("Index", "Cart");
                }

                var vnpayResponse = _vnPayService.PaymentExecute(Request.Query);
                Console.WriteLine($"VNPay Response - Success: {vnpayResponse.Success}, OrderId: {vnpayResponse.OrderId}");

                if (!vnpayResponse.Success)
                {
                    Console.WriteLine("Payment validation failed");
                    TempData["ErrorMessage"] = "Xác thực thanh toán thất bại";
                    return RedirectToAction("Index", "Cart");
                }

                // Find and update order
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.ORD_OrderCode.Equals(vnpayResponse.OrderId));
                    
                if (order == null)
                {
                    Console.WriteLine($"Order not found: {vnpayResponse.OrderId}");
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction("Index", "Cart");
                }

                if (vnpayResponse.VnPayResponseCode == "00")
                {
                    Console.WriteLine("Payment successful, updating order status");
                    order.ORD_Status = OrderStatusEnum.Processing;
                    await _context.SaveChangesAsync();

                    // Clear cart after successful payment
                    HttpContext.Session.Remove("Cart");
                    Response.Cookies.Delete("ShippingPrice");
                    Response.Cookies.Delete("CouponTitle");

                    TempData["SuccessMessage"] = "Thanh toán thành công";
                    return RedirectToAction("Success", "Checkout");
                }
                else
                {
                    Console.WriteLine($"Payment failed with response code: {vnpayResponse.VnPayResponseCode}");
                    order.ORD_Status = OrderStatusEnum.Failed;
                    await _context.SaveChangesAsync();
                    TempData["ErrorMessage"] = $"Thanh toán thất bại. Mã lỗi: {vnpayResponse.VnPayResponseCode}";
                    return RedirectToAction("Index", "Cart");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PaymentCallback: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình xử lý thanh toán";
                return RedirectToAction("Index", "Cart");
            }
        }

    }
}
