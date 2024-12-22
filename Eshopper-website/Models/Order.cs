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

		[Required(ErrorMessage = "Please enter order code!"), MinLength(5), MaxLength(255)]
		[DisplayName("Code")]
		public required string ORD_OrderCode { get; set; }

		[Required(ErrorMessage = "Please enter order description!"), MinLength(5), MaxLength(255)]
		[DisplayName("Description")]
		public required string ORD_Description { get; set; }

		[Required(ErrorMessage = "Please enter order status!")]
		[DisplayName("Status")]
		public required OrderStatusEnum ORD_Status { get; set; } = OrderStatusEnum.Pending;

        [Required(ErrorMessage = "Please enter order shipping cost!")]
		[DisplayName("Shipping Cost")]
		public required decimal ORD_ShippingCost { get; set; }

		[MaxLength(255)]
		[DisplayName("Coupon Code")]
		public string? ORD_CouponCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter payment method!")]
		[DisplayName("Payment Method")]
		public required OrderPaymentMethodEnum ORD_PaymentMethod { get; set; } = OrderPaymentMethodEnum.Cash;

        [Required(ErrorMessage = "Please enter order total price!")]
		[DisplayName("Total Price")]
        public required decimal ORD_TotalPrice { get; set; }

        [ForeignKey("MEM_ID")]
		public virtual Member? Member { get; set; }

		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

	}
}
