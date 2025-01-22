using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Models.VNPay
{
    public class PaymentResponseModel
    {
        public bool Success { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Email { get; set; }
    }
}
