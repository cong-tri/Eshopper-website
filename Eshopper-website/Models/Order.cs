using Eshopper_website.Utils.Enum.Order;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Orders")]
	public class account : BaseModel
	{
		public account() { }

		public account(int memId, string orderCode, string description)
		{
			MEM_ID = memId;
			ORD_OrderCode = orderCode;
			ORD_Description = description;
			CreatedDate = DateTime.Now;
			ORD_Status = OrderStatusEnum.Pending;
			ORD_PaymentMethod = OrderPaymentMethodEnum.Cash;
		}

		[Key]
		public int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter member id!")]
		[DisplayName("Member ID")]
		public int MEM_ID { get; set; }

		[Required(ErrorMessage = "Please enter order code!")]
		[MinLength(5, ErrorMessage = "Order code cannot exceed 5 characters!")]
		[MaxLength(255, ErrorMessage = "Order code cannot exceed 255 characters!")]
		[DisplayName("Code")]
		public string ORD_OrderCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter order description!")]
		[MinLength(5, ErrorMessage = "Order description cannot exceed 5 characters!")]
		[MaxLength(255, ErrorMessage = "Order description cannot exceed 255 characters!")]
		[DisplayName("Description")]
		public string ORD_Description { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter order status!")]
		[DisplayName("Status")]
		public OrderStatusEnum ORD_Status { get; set; } = OrderStatusEnum.Pending;

		[Required(ErrorMessage = "Please enter order shipping cost!")]
		[Range(1, 1000000, ErrorMessage = "Shipping price must be a positive number!")]
		[DisplayName("Shipping Cost")]
		public decimal ORD_ShippingCost { get; set; }

		[MaxLength(255, ErrorMessage = "Coupon code cannot exceed 255 characters!")]
		[DisplayName("Coupon Code")]
		public string? ORD_CouponCode { get; set; }

		[Required(ErrorMessage = "Please enter payment method!")]
		[DisplayName("Payment Method")]
		public OrderPaymentMethodEnum ORD_PaymentMethod { get; set; } = OrderPaymentMethodEnum.Cash;

		[Required(ErrorMessage = "Please enter order total price!")]
		[Range(1, 100000000, ErrorMessage = "Total price must be a positive number!")]
		[DisplayName("Total Price")]
		public decimal ORD_TotalPrice { get; set; }

		[DisplayName("Last Modified")]
		public DateTime? ModifiedDate { get; set; }

		[DisplayName("Modified By")]
		[MaxLength(255)]
		public string? ModifiedBy { get; set; }

		[ForeignKey("MEM_ID")]
		public virtual Member? Member { get; set; }

		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
	}
}
