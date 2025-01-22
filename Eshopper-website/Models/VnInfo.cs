using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("VnInfos")]
	public class VnInfo : BaseModel
	{
		[Key]
		public int ID { get; set; }

		[Required(ErrorMessage = "Please enter order description"), MinLength(5), MaxLength(255)]
		[DisplayName("Order Description")]
		public required string OrderDescription { get; set; }

		[Required(ErrorMessage = "Please enter transaction id")]
		[DisplayName("Transaction ID")]
		public required string TransactionId { get; set; }

		[Required(ErrorMessage = "Please enter order id")]
		[DisplayName("Order ID")]
		public required string OrderId { get; set; }

		[Required(ErrorMessage = "Please enter payment method")]
		[DisplayName("Payment Method")]
		public required string PaymentMethod { get; set; }

		[Required(ErrorMessage = "Please enter payment id")]
		[DisplayName("Payment ID")]
		public required string PaymentId { get; set; }

		public decimal Amount { get; set; }

    }
}
