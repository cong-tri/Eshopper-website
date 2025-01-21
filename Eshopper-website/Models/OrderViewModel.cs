using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Order;

namespace Eshopper_website.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusEnum Status { get; set; }
        public OrderPaymentMethodEnum PaymentMethod { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }

    public class OrderDetailViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
    }
} 