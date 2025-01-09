using Azure.Core;
using Eshopper_website.Models.VNPay;
using Eshopper_website.Services.VNPay;
using Microsoft.AspNetCore.Mvc;

namespace Eshopper_website.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {
                
            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        

    }
}
