using Eshopper_website.Utils.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class ProductDTO
    {
        public int PRO_ID { get; set; }

        public required int CAT_ID { get; set; }

        public required int BRA_ID { get; set; }

        public required string PRO_Name { get; set; }

        public required string PRO_Description { get; set; }

        public string? PRO_Slug { get; set; } = string.Empty;

        public required decimal PRO_Price { get; set; }

        public IFormFile? PRO_Image { get; set; }

        public required int PRO_Quantity { get; set; }

        public required ProductStatusEnum PRO_Status { get; set; }

        public required decimal PRO_CapitalPrice { get; set; }
    }
}
