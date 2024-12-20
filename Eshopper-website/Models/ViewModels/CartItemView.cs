namespace Eshopper_website.Models.ViewModels
{
    public class CartItemView
    {
        public List<CartItem>? CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
