using Eshopper_website.Utils.Enum.Order;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Orders")]
	public class Order : BaseModel
	{
		[Key]
		public int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter member id!")]
		[DisplayName("Member ID")]
		public required int MEM_ID { get; set; }

		[Required(ErrorMessage = "Please enter order code!")]
		[MinLength(5, ErrorMessage = "Order code cannot exceed 5 characters!")]
		[MaxLength(255, ErrorMessage = "Order code cannot exceed 255 characters!")]
		[DisplayName("Code")]
		public required string ORD_OrderCode { get; set; }

		[Required(ErrorMessage = "Please enter order description!")]
		[MinLength(5, ErrorMessage = "Order description cannot exceed 5 characters!")]
		[MaxLength(255, ErrorMessage = "Order description cannot exceed 255 characters!")]
		[DisplayName("Description")]
		public required string ORD_Description { get; set; }

		[Required(ErrorMessage = "Please enter order status!")]
		[DisplayName("Status")]
		public required OrderStatusEnum ORD_Status { get; set; } = OrderStatusEnum.Pending;

        [Required(ErrorMessage = "Please enter order shipping cost!")]
		[Range(1, 1000000, ErrorMessage = "Shipping price must be a positive number!")]
		[DisplayName("Shipping Cost")]
		public required decimal ORD_ShippingCost { get; set; }

		[MaxLength(255, ErrorMessage = "Coupon code cannot exceed 255 characters!")]
		[DisplayName("Coupon Code")]
		public string? ORD_CouponCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter payment method!")]
		[DisplayName("Payment Method")]
		public required OrderPaymentMethodEnum ORD_PaymentMethod { get; set; } = OrderPaymentMethodEnum.Cash;

        [Required(ErrorMessage = "Please enter order total price!")]
		[Range(1, 100000000, ErrorMessage = "Total price must be a positive number!")]
		[DisplayName("Total Price")]
        public required decimal ORD_TotalPrice { get; set; }

        [DisplayName("Shipping Address")]
        [MaxLength(120, ErrorMessage = "Shipping address cannot exceed 120 characters!")]
        public string? ORD_ShipAddress { get; set; }

        [DisplayName("Is GHN")]
        public OrderIsGHNEnum ORD_IsGHN { get; set; } = OrderIsGHNEnum.Inactive;

        [ForeignKey("MEM_ID")]
		public virtual Member? Member { get; set; }

		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

	}
}
