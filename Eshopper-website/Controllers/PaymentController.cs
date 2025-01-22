using Eshopper_website.Models;
using Eshopper_website.Models.VNPay;
using Eshopper_website.Services.Momo;
using Eshopper_website.Services.VNPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models.DataContext;
using Eshopper_website.Utils.Extension;
using Eshopper_website.Utils.Enum.Order;

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

        //[HttpPost]
        //public async Task<IActionResult> CreatePaymentUrlVnpay([FromForm] PaymentInformationModel model)
        //{
        //    try
        //    {
        //        // Validate amount
        //        if (model.Amount <= 0)
        //        {
        //            TempData["ErrorMessage"] = "Số tiền phải lớn hơn 0";
        //            return RedirectToAction("Index", "Cart");
        //        }

        //        var userInfo = HttpContext.Session.Get<UserInfo>("userInfo");
        //        if (userInfo == null)
        //        {
        //            TempData["ErrorMessage"] = "Vui lòng đăng nhập để thanh toán";
        //            return RedirectToAction("Login", "Account");
        //        }

        //        // Convert to integer amount for VNPay (no decimals)
        //        var intAmount = (long)(model.Amount * 100); // VNPay requires amount * 100
        //        Console.WriteLine($"Amount for VNPay: {intAmount}");

        //        var order = new Order()
        //        {
        //            MEM_ID = userInfo.MEM_ID,
        //            ORD_OrderCode = DateTime.Now.Ticks.ToString(), // Use ticks for unique order code
        //            ORD_Description = $"Thanh toán đơn hàng QR VNPay",
        //            ORD_Status = OrderStatusEnum.WaitingForPayment,
        //            ORD_PaymentMethod = (int)OrderPaymentMethodEnum.VNPay,
        //            ORD_TotalPrice = model.Amount,
        //            CreatedDate = DateTime.Now,
        //            CreatedBy = userInfo.ACC_Username
        //        };

        //        _context.Orders.Add(order);
        //        await _context.SaveChangesAsync();

        //        // Update payment model
        //        model.Amount = intAmount;
        //        model.OrderDescription = $"Thanh toan don hang: {order.ORD_OrderCode}";
        //        model.OrderType = "other";
        //        model.Name = userInfo.ACC_DisplayName ?? userInfo.ACC_Username;

        //        // Get the base URL of the application
        //        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        //        // Set the return URL explicitly
        //        model.ReturnUrl = $"{baseUrl}/Payment/PaymentCallbackVnpay";

        //        Console.WriteLine($"Creating payment URL with return URL: {model.ReturnUrl}");
        //        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        //        Console.WriteLine($"Generated payment URL: {url}");

        //        return Redirect(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in CreatePaymentUrlVnpay: {ex.Message}");
        //        TempData["ErrorMessage"] = $"Lỗi khi xử lý thanh toán: {ex.Message}";
        //        return RedirectToAction("Index", "Cart");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay([FromQuery] string vnp_ResponseCode, [FromQuery] string vnp_TxnRef)
        {
            try
            {
                Console.WriteLine($"=== VNPay Callback Started ===");
                Console.WriteLine($"Response Code: {vnp_ResponseCode}");
                Console.WriteLine($"Transaction Ref: {vnp_TxnRef}");

                // Kiểm tra response code trước
                if (vnp_ResponseCode != "00")
                {
                    Console.WriteLine($"Payment failed with response code: {vnp_ResponseCode}");
                    TempData["ErrorMessage"] = $"Thanh toán thất bại. Mã lỗi: {vnp_ResponseCode}";
                    return RedirectToAction("Index", "Cart");
                }

                var order = await _context.Orders
                    .Include(o => o.Member)
                    .FirstOrDefaultAsync(o => o.ORD_OrderCode == vnp_TxnRef);

                if (order == null)
                {
                    Console.WriteLine($"Order not found: {vnp_TxnRef}");
                    TempData["error"] = "Không tìm thấy thông tin đơn hàng";
                    return RedirectToAction("Index", "Cart");
                }

                Console.WriteLine($"Order found - ID: {order.ORD_ID}, Status: {order.ORD_Status}");

                // Cập nhật trạng thái đơn hàng
                order.ORD_Status = OrderStatusEnum.Processing;
                await _context.SaveChangesAsync();

                // Clear cart and cookies after successful payment
                HttpContext.Session.Remove("Cart");
                Response.Cookies.Delete("ShippingPrice");

                // Redirect to success page with order details
                Console.WriteLine($"Redirecting to Success page with orderId: {order.ORD_ID}");
                TempData["SuccessMessage"] = "Thanh toán đơn hàng thành công!";
                return RedirectToAction("Success", "Checkout", new { orderId = order.ORD_ID });
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
