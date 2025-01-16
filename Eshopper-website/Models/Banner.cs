using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Banners")]
	public class Banner : BaseModel
	{
		[Key]
		public int BAN_ID { get; set; }

		[Required(ErrorMessage = "Please enter banner title")]
		[MinLength(5, ErrorMessage = "Title must be at least 5 characters long!")]
		[MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters!")]
		[DisplayName("Title")]
		public required string BAN_Title { get; set; }

		[DisplayName("Image")]
		public string? BAN_Image { get; set; }

        [MaxLength(255, ErrorMessage = "URL cannot exceed 255 characters!")]
		[DataType(DataType.Url, ErrorMessage = "Please enter correct format url!")]
        [DisplayName("URL")]
		public string? BAN_Url { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter banner display order")]
		[Range(1, 6, ErrorMessage = "Display order must be a positive number!")]
		[DisplayName("Display Order")]
		public required int BAN_DisplayOrder { get; set; }

	}
}
