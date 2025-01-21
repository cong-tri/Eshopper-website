using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Models.ViewModels
{
    public class CheckoutView
    {
        public int MEM_ID { get; set; } 

        [Required(ErrorMessage = "Please enter fullname of buyer!")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Please enter phone number of buyer!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter the correct format phone number!")]
        [MinLength(10, ErrorMessage = "Phone must be at least 10 characters long!")]
        [MaxLength(12, ErrorMessage = "Phone cannot exceed 12 characters!")]
        public required string Phone { get; set; }

		[Required(ErrorMessage = "Please enter email of buyer!")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter the correct format email!")]
		public required string Email { get; set; }

        [Required(ErrorMessage = "Please choose province name!")]
        [DisplayName("Province")]
        public int? ToProviceId { get; set; }

        [Required(ErrorMessage = "Please choose district name!")]
        [DisplayName("District")]
        public int? ToDistrictId { get; set; }

        [Required(ErrorMessage = "Please choose ward name!")]
        [DisplayName("Ward")]
        public string? ToWardCode { get; set; }

        [Required(ErrorMessage = "Please enter address of buyer!")]
        [MinLength(10, ErrorMessage = "Address must be at least 10 characters long!")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters!")]
        [DisplayName("Address")]
        public string? ToAddress { get; set; }

        [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters!")]
        public string? Note { get; set; }

        [DisplayName("Payment")]
        public int PaymentMethod { get; set; }
        public decimal ShippingPrice { get; set; }

        public List<CartItem>? CartItems { get; set; }
        //public decimal GrandTotal { get; set; }
        //public string? CouponCode { get; set; }
        //public decimal? DiscountAmount { get; set; }
        //public decimal? FinalTotal => GrandTotal - DiscountAmount;
    }
}
