
using Eshopper_website.Models;
using Eshopper_website.Models.VNPay;
using Eshopper_website.Services.Momo;
using Eshopper_website.Services.VNPay;
using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly IMomoService _momoService;
        public PaymentController(IVnPayService vnPayService, IMomoService momoService)
        {    
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
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
    }
}
