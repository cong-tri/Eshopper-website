using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class BannerDTO
    {
        public int BAN_ID { get; set; }
        public required string BAN_Title { get; set; }
        public IFormFile? BAN_Image { get; set; }
        public string? BAN_Url { get; set; } = string.Empty;
        public required int BAN_DisplayOrder { get; set; }
    }
}
