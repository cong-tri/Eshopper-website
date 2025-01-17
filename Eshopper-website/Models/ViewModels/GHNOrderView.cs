using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Models.ViewModels
{
    public class GHNOrderView
    {
        [Required(ErrorMessage = "Please enter order code!")]
        [Display(Name = "Order Code")]
        public string ClientOrderCode { get; set; }

        [Required(ErrorMessage = "Please enter receiver name!")]
        [Display(Name = "Receiver Name")]
        public string ToName { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter the correct format phone!")]
        [Display(Name = "Phone Number")]
        public string ToPhone { get; set; }

        [Required(ErrorMessage = "Please enter receiver address!")]
        [Display(Name = "Receiver Address")]
        public string ToAddress { get; set; }

        [Required(ErrorMessage = "Please enter ward code!")]
        [Display(Name = "Ward Code")]
        public string ToWardCode { get; set; }

        [Required(ErrorMessage = "Please enter district id!")]
        [Display(Name = "District ID")]
        public int ToDistrictId { get; set; }

        [Required]
        [Display(Name = "Provice ID")]
        public int ToProviceId { get; set; }

        [Required]
        [Display(Name = "Weight (grams)")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Length (cm)")]
        public int Length { get; set; }

        [Required]
        [Display(Name = "Width (cm)")]
        public int Width { get; set; }

        [Required]
        [Display(Name = "Height (cm)")]
        public int Height { get; set; }
    }
}
