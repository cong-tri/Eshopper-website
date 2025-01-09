namespace Eshopper_website.Models.VNPay
{
    public class PaymentInformationModel
    {
        public required string OrderType { get; set; }
        public double Amount { get; set; }
        public required string OrderDescription { get; set; }
        public required string Name { get; set; }
    }
}
