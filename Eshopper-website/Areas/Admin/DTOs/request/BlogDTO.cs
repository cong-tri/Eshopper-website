using Eshopper_website.Utils.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class BlogDTO
    {
   
        public int BLG_ID { get; set; }
  
        public required string BLG_Title { get; set; }
     
        public string? BLG_Slug { get; set; }

        public required string BLG_Content { get; set; }

        public IFormFile? BLG_Image { get; set; }

        public required string BLG_AuthorName { get; set; }

        public required DateTime BLG_PublishedAt { get; set; } = DateTime.Now;

        public required BlogStatusEnum BLG_Status { get; set; }
    }
}
