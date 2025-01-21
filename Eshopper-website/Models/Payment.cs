using Eshopper_website.Utils.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("PayMents")]
	public class Payment : BaseModel
	{
		[Key]
		public int PAY_ID { get; set; }

		[Required(ErrorMessage = "Please enter payment method name")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters long!")]
        [MaxLength(15, ErrorMessage = "Name cannot exceed 15 characters!")]
        [DisplayName("Name")]
		public required string PAY_Name { get; set; }

		[Required(ErrorMessage = "Please enter payment method description")]
        [MinLength(5, ErrorMessage = "Description must be at least 5 characters long!")]
        [MaxLength(50, ErrorMessage = "Description cannot exceed 50 characters!")]
        [DisplayName("Description")]
		public required string PAY_Description { get; set; }

		[Required(ErrorMessage = "Please enter payment method status"), Column(TypeName = "INT")]
		[DisplayName("Status")]
		public PaymentStatusEnum PAY_Status { get; set; }
	}
}
