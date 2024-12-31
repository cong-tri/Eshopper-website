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

        [Required(ErrorMessage = "Please enter blog title!")]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long!")]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 2555 characters!")]
        [DisplayName("Title")]
        public required string BLG_Title { get; set; }

        [MaxLength(255, ErrorMessage = "Slug cannot exceed 255 characters!")]
        [DisplayName("Slug")]
        public string? BLG_Slug { get; set; }

        [Required(ErrorMessage = "Please enter blog content!")]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long!")]
        [DisplayName("Content")]
        public required string BLG_Content { get; set; }

        [DisplayName("Image")]
        public string? BLG_Image { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter blog author name!")]
        [MinLength(5, ErrorMessage = "Author name must be at least 5 characters long!")]
        [MaxLength(50, ErrorMessage = "Author name cannot exceed 50 characters!")]
        [DisplayName("Author Name")]
        public required string BLG_AuthorName { get; set; }

        [Required(ErrorMessage = "Please enter blog published at date!"), Column(TypeName = "DATETIME")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter the date published!")]
        [DisplayName("Published At")]
        public required DateTime BLG_PublishedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter blog status!"), Column(TypeName = "INT")]
        [DisplayName("Status")]
        public required BlogStatusEnum BLG_Status { get; set; }
    }
} 