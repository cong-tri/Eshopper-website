using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using Eshopper_website.Models.GHN;
using RestSharp;
using Eshopper_website.Models.Recaptcha;
using Microsoft.Extensions.Options;

namespace Eshopper_website.Services.Recaptcha
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly ILogger<RecaptchaService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IOptions<Recaptcha_Setting> _settings;
        public string LastError { get; set; }
        public RecaptchaService(ILogger<RecaptchaService> logger, 
            IConfiguration configuration, 
            IOptions<Recaptcha_Setting> settings, 
            string lastError, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _settings = settings;
            LastError = lastError;
            _httpClient = httpClient;
        }
        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                LastError = "";
                if (string.IsNullOrEmpty(token))
                {
                    LastError = "Token reCAPTCHA is empty!";
                    _logger.LogWarning(LastError);
                    return false;
                }

                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_settings.Value.SecretKey}&response={token}";

                var response = await _httpClient.PostAsync(url, null);
                //var request = new RestRequest() { Method = Method.Post };

                //var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    LastError = $"Lỗi kết nối: {response.StatusCode}";
                    _logger.LogError(LastError);
                    return false;
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var responseData = JsonSerializer.Deserialize<Recaptcha_Response>(responseString, options);

                if (responseData == null)
                {
                    LastError = "Cannot read response from Google.";
                    _logger.LogError(LastError);
                    return false;
                }

                if (!responseData.Success && responseData.ErrorCodes != null && responseData.ErrorCodes.Length > 0)
                {
                    LastError = GetErrorMessage(responseData.ErrorCodes);
                    _logger.LogWarning(LastError);
                }

                return responseData.Success;
            }
            catch (Exception ex)
            {
                LastError = $"Failed authorize reCAPTCHA: {ex.Message}";
                _logger.LogError(ex, LastError);
                return false;
            }
        }

        private string GetErrorMessage(string[] errorCodes)
        {
            var errorMessages = new Dictionary<string, string>
            {
                {"missing-input-secret", "Thiếu secret key"},
                {"invalid-input-secret", "Secret key không hợp lệ"},
                {"missing-input-response", "Thiếu token response"},
                {"invalid-input-response", "Token response không hợp lệ"},
                {"bad-request", "Request không hợp lệ"},
                {"timeout-or-duplicate", "Token đã hết hạn hoặc đã được sử dụng"}
            };

            var errors = errorCodes.Select(code => errorMessages.ContainsKey(code) ? errorMessages[code] : code);
            return string.Join(", ", errors);
        }
    }
} 