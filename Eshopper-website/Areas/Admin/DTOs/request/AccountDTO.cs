using Eshopper_website.Models;
using Eshopper_website.Utils.Enum;
using Eshopper_website.Utils.Enum.Member;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshopper_website.Areas.Admin.DTOs.request
{
	public class AccountDTO
	{
		public int ACC_ID { get; set; }
		public string? ACC_Username { get; set; }
		public string? ACC_DisplayName { get; set; }
		public string? ACC_Email { get; set; }
		public string? ACC_Phone { get; set; }
		public AccountStatusEnum ACC_Status { get; set; } = AccountStatusEnum.Inactive;
		public MemberStatusEnum MEM_Status { get; set; } = MemberStatusEnum.Active;

		public AccountDTO() { }

		public AccountDTO(Account account, MemberStatusEnum memberStatusEnum) 
		{
			ACC_ID = account.ACC_ID;
			ACC_Username = account.ACC_Username;
			ACC_DisplayName = account.ACC_DisplayName;
			ACC_Email = account.ACC_Email;
			ACC_Phone = account.ACC_Phone;
			ACC_Status = account.ACC_Status;
			MEM_Status = memberStatusEnum;
		}
	}
}

