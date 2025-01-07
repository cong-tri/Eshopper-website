using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Ratings")]
	public class Rating : BaseModel
	{
		[Key]
		public int RAT_ID { get; set; }

		[Required(ErrorMessage = "Please enter product id")]
		[DisplayName("Product ID")]
		public required int PRO_ID { get; set; }

		[MinLength(5), MaxLength(255)]
		[DisplayName("Comment")]
		public string? RAT_Comment { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter name!"), MinLength(5), MaxLength(255)]
		[DisplayName("Name")]
		public required string RAT_Name { get; set; }

		[Required(ErrorMessage = "Please enter email!"), MinLength(10), MaxLength(255)]
		[DisplayName("Email")]
		public required string RAT_Email { get; set; }

		[Required(ErrorMessage = "Please enter star!")]
		[DisplayName("Star")]
		public required int RAT_Star { get; set; }

		[ForeignKey("PRO_ID")]
		public virtual Product? Product { get; set; }
	}
}
