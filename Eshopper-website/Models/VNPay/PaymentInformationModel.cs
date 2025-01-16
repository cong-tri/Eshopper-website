namespace Eshopper_website.Models.VNPay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; } = "other";
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
