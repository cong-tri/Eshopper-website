using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Eshopper_website.Utils.Enum;

namespace Eshopper_website.Models
{
	[Table("Brands")]
	public class Brand : BaseModel
	{
		[Key]
		public int BRA_ID { get; set; }

		[Required(ErrorMessage = "Please enter brand name!"), MinLength(5), MaxLength(255)]
		[DisplayName("Name")]
		public required string BRA_Name { get; set; }

		[Required(ErrorMessage = "Please enter brand description!"), MinLength(5), MaxLength(255)]
		[DisplayName("Description")]
		public required string BRA_Description { get; set; }

		[MaxLength(255)]
		[DisplayName("Slug")]
		public string? BRA_Slug { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter brand status")]
		[DisplayName("Status")]
		public required BrandStatusEnum BRA_Status { get; set; }

		[Required(ErrorMessage = "Please enter brand display order!")]
		[Range(1, int.MaxValue, ErrorMessage = "Display order must be a positive number!")]
		[DisplayName("Display Order")]
		[Range(1, int.MaxValue, ErrorMessage = "Display order must be a positive number!")]
		public required int BRA_DisplayOrder { get; set; }

		public virtual ICollection<Product>? Products { get; set; }
	}
}
