using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Eshopper_website.Utils.Enum;

namespace Eshopper_website.Models
{
	[Table("Menus")]
	public class Menu : BaseModel
	{
		[Key]
		public int MEN_ID { get; set; }

		[DisplayName("Parent ID")]
		public int? PARENT_ID { get; set; }

		[Required(ErrorMessage = "Please enter menu title")]
		[MinLength(2, ErrorMessage = "Title cannot exceed 2 characters!")]
		[MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters!")]
		[DisplayName("Title")]
		public required string MEN_Title { get; set; }

		[Required(ErrorMessage = "Please enter menu display order")]
		[DisplayName("Display Order")]
        [Range(1, 30, ErrorMessage = "Menu quantity must be a positive number!")]
        public required int MEN_DisplayOrder { get; set; }

		[MaxLength(50, ErrorMessage = "Icon cannot exceed 50 characters long!")]
		[DisplayName("Icon")]
		public string? MEN_Icon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter menu status")]
        [DisplayName("Status")]
        public required MenuStatusEnum MEN_Status { get; set; } = MenuStatusEnum.User;

        [Required(ErrorMessage = "Please enter menu controller")]
        [MinLength(2, ErrorMessage = "Controller must be at lease 2 characters long!")]
        [MaxLength(50, ErrorMessage = "Controller cannot exceed 50 characters long!")]
        [DisplayName("Controller")]
        public required string MEN_Controller { get; set; }

        [ForeignKey("PARENT_ID")]  
        public virtual Menu? Parent { get; set; }
		public virtual ICollection<Menu>? Childrens { get; set; }
	}
}
