using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Eshopper_website.Utils.Enum;

namespace Eshopper_website.Models
{
	[Table("Coupons")]
	public class Coupon : BaseModel
	{
		[Key]
		public int COUP_ID { get; set; }

		[Required(ErrorMessage = "Please enter coupon name!")]
		[MinLength(5, ErrorMessage = "Name must be at least 5 characters long!")]
		[MaxLength(255, ErrorMessage = "Name cannot exceed 255 characters!")]
		[DisplayName("Name")]
		public required string COUP_Name { get; set; }

		[Required(ErrorMessage = "Please enter coupon description!"), MinLength(5), MaxLength(255)]
		[DisplayName("Description")]
		public required string COUP_Description { get; set; }

		[Required(ErrorMessage = "Please enter coupon status!")]
		[DisplayName("Status"), Column(TypeName = "INT")]
		public required CouponStatusEnum COUP_Status { get; set; }

		[Required(ErrorMessage = "Please enter coupon quantity!")]
        [Range(1, 100, ErrorMessage = "Coupon quantity must be a positive number!")]
        [DisplayName("Quantity")]
		public required int COUP_Quantity { get; set; }	

		[Required(ErrorMessage = "Please enter the date start for coupon!")]
		[DisplayName("Date Start")]
		public required DateTime COUP_DateStart { get; set; } = DateTime.Now;

		[Required(ErrorMessage = "Please enter the date expire for coupon!")]
		[DisplayName("Date Expire")]
		public required DateTime COUP_DateExpire { get; set; } = DateTime.Now;
	}
}
