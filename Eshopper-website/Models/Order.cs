﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Orders")]
	public class Order : BaseModel
	{
		[Key]
		public int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter order code!"), MinLength(5), MaxLength(255)]
		[DisplayName("Code")]
		public required string ORD_OrderCode { get; set; }

		[Required(ErrorMessage = "Please enter order description!"), MinLength(5), MaxLength(255)]
		[DisplayName("Description")]
		public required string ORD_Description { get; set; }

		[Required(ErrorMessage = "Please enter username!"), MinLength(5), MaxLength(255)]
		[DisplayName("UserName")]
		public required string USER_UserName { get; set; }

		[Required(ErrorMessage = "Please enter order status!")]
		[DisplayName("Status")]
		public required int ORD_Status {  get; set; }

		[Required(ErrorMessage = "Please enter order shipping cost!")]
		[DisplayName("Shipping Cost")]
		public required decimal ORD_ShippingCost { get; set; }

		[MaxLength(255)]
		[DisplayName("Coupon Code")]
		public string? ORD_CouponCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter payment method!")]
		[DisplayName("Payment Method")]
		public required int ORD_PaymentMethod { get; set; }

		//[ForeignKey("USER_UserName")]
		//public virtual User? User { get; set; }

		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

	}
}
