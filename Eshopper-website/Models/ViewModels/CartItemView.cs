namespace Eshopper_website.Models.ViewModels
{
    public class CartItemView
    {
        public List<CartItem>? CartItems { get; set; }
        public decimal GrandTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalTotal => GrandTotal - DiscountAmount;
    }
}
