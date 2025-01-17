using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Eshopper_website.Models.ViewModels
{
    public class ProductDetailsView
    {
        public Product? ProductDetail { get; set; }
        public string? Comment { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public ICollection<Rating> Ratings { get; set; } = [];
    }
}
