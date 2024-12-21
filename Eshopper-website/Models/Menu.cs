using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshopper_website.Models
{
	[Table("Menus")]
	public class Menu : BaseModel
	{
		[Key]
		public int MEN_ID { get; set; }

		[DisplayName("Parent ID")]
		public int? PARENT_ID { get; set; }

		[Required(ErrorMessage = "Please enter menu title"), MinLength(2), MaxLength(255)]
		[DisplayName("Title")]
		public required string MEN_Title { get; set; }

		[MaxLength(255)]
		[DisplayName("URL")]
		public string? MEN_Url { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter menu display order")]
		[DisplayName("Display Order")]
		public required int MEN_DisplayOrder { get; set; }

        [Required(ErrorMessage = "Please enter menu icon"),MaxLength(255)]
        [DisplayName("Icon")]
        public required string MEN_Icon { get; set; }

        [ForeignKey("PARENT_ID")]  
        public virtual Menu? Parent { get; set; }
		public virtual ICollection<Menu>? Childrens { get; set; }
	}
}
