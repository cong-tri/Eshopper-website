using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("VnInfos")]
	public class VnInfo
	{
		[Key]
		public int VNIN_ID { get; set; }

		[Required(ErrorMessage = "Please enter order description"), MinLength(5), MaxLength(255)]
		[DisplayName("Order Description")]
		public required string ORD_Description { get; set; }

		[Required(ErrorMessage = "Please enter transaction id")]
		[DisplayName("Transaction ID")]
		public required int TRANS_ID { get; set; }

		[Required(ErrorMessage = "Please enter order id")]
		[DisplayName("Order ID")]
		public required int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter payment method")]
		[DisplayName("Payment Method")]
		public required int VNIN_PaymentMethod { get; set; }

		[Required(ErrorMessage = "Please enter payment id")]
		[DisplayName("Payment ID")]
		public required int PAY_ID { get; set; }

		[ForeignKey("ORD_ID")]
		public virtual Order? Order { get; set; }

		//[ForeignKey("PAY_ID")]
		//public virtual Payment? Payment { get; set; }

		//[ForeignKey("TRANS_ID")]
		//public virtual Transaction? Transaction { get; set; }
	}
}
