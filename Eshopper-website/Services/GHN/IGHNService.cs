using Eshopper_website.Models.GHN;
using Eshopper_website.Models.GHN.Response;
namespace Eshopper_website.Services.GHN
{
    public interface IGHNService
    {
        Task<GHN_Order_Response> CreateOrderAsync(GHN_Order ghn_order);
		Task<GHN_Province_Response> GetProvinceAsync();
		Task<GHN_District_Response> GetDistrictAsync(int? province_id);
		Task<GHN_Ward_Response> GetWardAsync(int district_id);
    }
}
