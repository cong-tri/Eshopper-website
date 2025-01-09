using Eshopper_website.Utils.Enum;

namespace Eshopper_website.Models
{
    public class UserInfo : BaseModel
    {
        public int ACC_ID { get; set; }
        public int MEM_ID { get; set; }
		public int ACR_ID { get; set; }
        public string? ACC_Username { get; set; }
        public string? ACC_DisplayName { get; set; }
        public string? ACC_Email { get; set; }
        public string? ACC_Phone { get; set; }
        public AccountStatusEnum ACC_Status { get; set; } = AccountStatusEnum.Inactive;

        public UserInfo() { }

        public UserInfo(Account account, int acr_id, int mem_id)
        {
            ACC_ID = account.ACC_ID;
            ACR_ID = acr_id;
            MEM_ID = mem_id;
            ACC_Username = account.ACC_Username;
            ACC_DisplayName = account.ACC_DisplayName;
            ACC_Email = account.ACC_Email;
            ACC_Phone = account.ACC_Phone;
            ACC_Status = account.ACC_Status;
            CreatedBy = account.CreatedBy;
            CreatedDate = account.CreatedDate;
            UpdatedBy = account.UpdatedBy;
            UpdatedDate = account.UpdatedDate;
        }
    }
}
