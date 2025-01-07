using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Models.ViewModels
{
    public class ProductDetailsView
    {
        public Product ProductDetail { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập bình luận")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập bình luận")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        public string Email { get; set; }
    }
}
