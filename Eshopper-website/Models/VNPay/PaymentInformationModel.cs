﻿namespace Eshopper_website.Models.VNPay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; } = "other";
        public double Amount { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
