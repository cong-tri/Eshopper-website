using Eshopper_website.Utils.Enum.Member;
using System.ComponentModel;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class ProfileDTO
    {
        public int ACC_ID { get; set; }
        public int MEM_ID { get; set; }

        [DisplayName("Display Name")]

        public required string ACC_DisplayName { get; set; }

        [DisplayName("Phone")]

        public required string MEM_Phone { get; set; }

        [DisplayName("Email")]
        public required string MEM_Email { get; set; }

        [DisplayName("Lastname")]
        public string? MEM_LastName { get; set; } = string.Empty;

        [DisplayName("Firstname")]
        public string? MEM_FirstName { get; set; } = string.Empty;

        [DisplayName("Gender")]
        public MemberGenderEnum MEM_Gender { get; set; } = MemberGenderEnum.Other;

        [DisplayName("Address")]
        public string? MEM_Address { get; set; } = string.Empty;
    }
}
