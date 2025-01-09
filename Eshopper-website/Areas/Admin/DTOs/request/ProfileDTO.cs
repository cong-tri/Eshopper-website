using Eshopper_website.Utils.Enum.Member;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class ProfileDTO
    {
        public int ACC_ID { get; set; }
        public int MEM_ID { get; set; }
        public string? ACC_DisplayName { get; set; }
        public required string MEM_Phone { get; set; }
        public required string MEM_Email { get; set; }
        public string? MEM_LastName { get; set; } = string.Empty;
        public string? MEM_FirstName { get; set; } = string.Empty;
        public MemberGenderEnum MEM_Gender { get; set; } = MemberGenderEnum.Other;
        public string? MEM_Address { get; set; } = string.Empty;
    }
}
