using Eshopper_website.Models.GHN;
using Eshopper_website.Models.GHN.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.Text.Json;

namespace Eshopper_website.Services.GHN
{
    public class GHNService : IGHNService
    {
		private const string DISTRICT_CACHE_KEY = "GHN_DISTRICTS";
		private readonly ILogger<GHNService> _logger;
        private readonly IOptions<GHN_Setting> _options;
        public GHNService(ILogger<GHNService> logger, IOptions<GHN_Setting> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<GHN_Order_Response> CreateOrderAsync(GHN_Order order)
        {
            try
            {
                var client = new RestClient($"{_options.Value.BaseUrl}/{GHN_Pathname.ghn_shipping_create_order}");
                var request = new RestRequest() { Method = Method.Post };

                request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                request.AddHeader("ShopId", _options.Value.ShopId);
                request.AddHeader("Token", _options.Value.Token);

                request.AddParameter("application/json", JsonConvert.SerializeObject(order), ParameterType.RequestBody);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadFromJsonAsync<GHN_Response>();
                    return JsonConvert.DeserializeObject<GHN_Order_Response>(response.Content); ;
                }

                //var errorContent = await response.Content.ReadAsStringAsync();
                //_logger.LogError($"GHN API Error: {errorContent}");
                throw new Exception($"Failed to create GHN order. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating GHN order");
                throw;
            }
        }

		public async Task<GHN_District_Response> GetDistrictAsync(int? province_id)
        {
            try
            {
                var client = new RestClient($"{_options.Value.BaseUrl}/{GHN_Pathname.ghn_shipping_get_district}");
                var request = new RestRequest() { Method = Method.Get };

                request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                request.AddHeader("ShopId", _options.Value.ShopId);
                request.AddHeader("Token", _options.Value.Token);

                if (province_id != null)
                {
                    var requestData = new
                    {
                        province_id
                    };
                    request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
                }

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GHN_District_Response>(response.Content); ;
                }

                throw new Exception($"Failed to get district. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get district");
                throw;
            }
        }

        public async Task<GHN_Province_Response> GetProvinceAsync()
        {
            try
            {
                var client = new RestClient($"{_options.Value.BaseUrl}/{GHN_Pathname.ghn_shipping_get_provice}");
                var request = new RestRequest() { Method = Method.Get };

                request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                request.AddHeader("ShopId", _options.Value.ShopId);
                request.AddHeader("Token", _options.Value.Token);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GHN_Province_Response>(response.Content);
                }

                throw new Exception($"Failed to get district. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get district");
                throw;
            }
        }

        public async Task<GHN_Ward_Response> GetWardAsync(int district_id)
        {
            try
            {
                var client = new RestClient($"{_options.Value.BaseUrl}/{GHN_Pathname.ghn_shipping_get_ward}");
                var request = new RestRequest() { Method = Method.Get };

                request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                request.AddHeader("ShopId", _options.Value.ShopId);
                request.AddHeader("Token", _options.Value.Token);

                var requestData = new
                {
                    district_id
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GHN_Ward_Response>(response.Content);
                }

                throw new Exception($"Failed to get district. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get district");
                throw;
            }
        }
        //public GHN_Response GHNExecuteAsync(IQueryCollection collection)
        //{
        //    int code = Int32.Parse(collection.First(s => s.Key == "code").Value);
        //    var message = collection.First(s => s.Key == "message").Value;
        //    //var data = collection.First(s => s.Key == "data").Value;
        //    return new GHN_Response()
        //    {
        //        Code = code,
        //        Message = message,
        //        //Data = data
        //    };
        //}
    }
}
