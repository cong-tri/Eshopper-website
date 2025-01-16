using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("MomoInfos")]
	public class MomoInfo : BaseModel
	{
		[Key]
		public int MOMO_ID { get; set; }

		[Required(ErrorMessage = "Please enter order id")]
		[DisplayName("Order ID")]
		public required int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter order info"), MinLength(5), MaxLength(255)]
		[DisplayName("Order Description")]
		public required string ORD_Description { get; set; }

		[Required(ErrorMessage = "Please enter fullname"), MinLength(5), MaxLength(255)]
		[DisplayName("FullName")]
		public required string MOMO_FullName { get; set; }

		[Required(ErrorMessage = "Please enter amount")]
		[DisplayName("Amount")]
		public required decimal MOMO_Amount { get; set; }

		[Required(ErrorMessage = "Please enter date paid")]
		[DisplayName("Date Paid")]
		public required DateTime MOMO_DatePaid { get; set; } = DateTime.Now;

		[ForeignKey("ORD_ID")]
		public virtual account? Order { get; set; }
	}
}
