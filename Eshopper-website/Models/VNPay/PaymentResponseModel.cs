namespace Eshopper_website.Models.VNPay
{
    public class PaymentResponseModel
    {
        public required string OrderDescription { get; set; }
        public required string TransactionId { get; set; }
        public required string OrderId { get; set; }
        public required string PaymentMethod { get; set; }
        public required string PaymentId { get; set; }
        public bool Success { get; set; }
        public required string Token { get; set; }
        public required string VnPayResponseCode { get; set; }
    }
}
