using Eshopper_website.Utils.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("Blogs")]
    public class Blog : BaseModel
    {
        [Key]
        public int BLG_ID { get; set; }

        [Required(ErrorMessage = "Please enter blog title!"), MinLength(5), MaxLength(255)]
        [DisplayName("Title")]
        public required string BLG_Title { get; set; }

        [MaxLength(255)]
        [DisplayName("Slug")]
        public string? BLG_Slug { get; set; }

        [Required(ErrorMessage = "Please enter blog content!"), MinLength(5)]
        [DisplayName("Content")]
        public required string BLG_Content { get; set; }

        [DisplayName("Image")]
        public string? BLG_Image { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter blog author name!"), MinLength(5), MaxLength(255)]
        [DisplayName("Author Name")]
        public required string BLG_AuthorName { get; set; }

        [Required(ErrorMessage = "Please enter blog published at date!"), Column(TypeName = "DATETIME")]
        [DisplayName("Published At")]
        public required DateTime BLG_PublishedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter blog status!"), Column(TypeName = "INT")]
        [DisplayName("Status")]
        public required BlogStatusEnum BLG_Status { get; set; }
    }
} 