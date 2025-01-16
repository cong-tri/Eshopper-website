using Eshopper_website.Models.VNPay;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace Eshopper_website.Libraries
{
    public class VnpayLibrary
    {
        private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new(new VnPayCompare());

        public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(hashSecret);

            var vnPay = new VnpayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    string safeValue = value.ToString() ?? string.Empty;
                    vnPay.AddResponseData(key, safeValue);
                }
            }

            try
            {
                var txnRef = vnPay.GetResponseData("vnp_TxnRef");
                var transactionNo = vnPay.GetResponseData("vnp_TransactionNo");
                var orderId = !string.IsNullOrEmpty(txnRef) ? Convert.ToInt64(txnRef) : 0;
                var vnPayTranId = !string.IsNullOrEmpty(transactionNo) ? Convert.ToInt64(transactionNo) : 0;
                var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
                var vnpSecureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value.ToString() ?? string.Empty;
                var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");
                var checkSignature = vnPay.ValidateSignature(vnpSecureHash, hashSecret);
                
                if (!checkSignature)
                    return new PaymentResponseModel
                    {
                        Success = false,
                        OrderDescription = string.Empty,
                        TransactionId = string.Empty,
                        OrderId = string.Empty,
                        PaymentMethod = string.Empty,
                        PaymentId = string.Empty,
                        Token = string.Empty,
                        VnPayResponseCode = string.Empty
                    };

                return new PaymentResponseModel
                {
                    Success = true,
                    PaymentMethod = "VnPay",
                    OrderDescription = orderInfo,
                    OrderId = orderId.ToString(),
                    PaymentId = vnPayTranId.ToString(),
                    TransactionId = vnPayTranId.ToString(),
                    Token = vnpSecureHash,
                    VnPayResponseCode = vnpResponseCode
                };
            }
            catch (Exception)
            {
                return new PaymentResponseModel
                {
                    Success = false,
                    OrderDescription = string.Empty,
                    TransactionId = string.Empty,
                    OrderId = string.Empty,
                    PaymentMethod = string.Empty,
                    PaymentId = string.Empty,
                    Token = string.Empty,
                    VnPayResponseCode = string.Empty
                };
            }
        }

        public string GetIpAddress(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if (remoteIpAddress is null)
                    return "127.0.0.1";

                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                        .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                return remoteIpAddress?.ToString() ?? "127.0.0.1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void AddRequestData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Console.WriteLine($"VnpayLibrary - Error: Empty key provided");
                throw new ArgumentNullException(nameof(key));
            }

            // Don't throw for null value, just convert to empty string
            var safeValue = value ?? string.Empty;
            
            Console.WriteLine($"VnpayLibrary - Adding request data: {key} = {safeValue}");
            
            if (!string.IsNullOrEmpty(safeValue))
            {
                // Special handling for amount
                if (key == "vnp_Amount")
                {
                    Console.WriteLine($"VnpayLibrary - Processing Amount: {safeValue}");
                    
                    // Parse amount using invariant culture to avoid regional formatting issues
                    if (decimal.TryParse(safeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                    {
                        // VNPay requires amount in VND (no decimals) * 100
                        var wholeAmount = (long)Math.Floor(amount);
                        var vnpayAmount = wholeAmount * 100;
                        var formattedAmount = vnpayAmount.ToString(CultureInfo.InvariantCulture);
                        
                        Console.WriteLine($"VnpayLibrary - Original Amount: {amount}");
                        Console.WriteLine($"VnpayLibrary - Whole Amount: {wholeAmount}");
                        Console.WriteLine($"VnpayLibrary - VNPay Amount (*100): {vnpayAmount}");
                        Console.WriteLine($"VnpayLibrary - Final Formatted Amount: {formattedAmount}");
                        
                        _requestData[key] = formattedAmount;
                    }
                    else
                    {
                        var error = $"Amount must be a valid number. Received: {safeValue}";
                        Console.WriteLine($"VnpayLibrary - Error: {error}");
                        throw new ArgumentException(error, nameof(value));
                    }
                }
                else
                {
                    _requestData[key] = safeValue;
                    Console.WriteLine($"VnpayLibrary - Added {key}: {safeValue}");
                }
            }
            else
            {
                Console.WriteLine($"VnpayLibrary - Skipped empty value for key: {key}");
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            // Don't throw for null value, just convert to empty string
            var safeValue = value ?? string.Empty;
            if (!string.IsNullOrEmpty(safeValue))
            {
                _responseData.Add(key, safeValue);
            }
        }

        public string GetResponseData(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
                
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            ArgumentNullException.ThrowIfNull(baseUrl);
            ArgumentNullException.ThrowIfNull(vnpHashSecret);

            var data = new StringBuilder();
            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();
            var signData = querystring;
            
            // Remove the last '&' for signing
            if (signData.Length > 0)
            {
                signData = signData[..^1];
            }

            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
            var paymentUrl = baseUrl + "?" + querystring + "vnp_SecureHash=" + vnpSecureHash;

            Console.WriteLine($"VnpayLibrary - Query string: {querystring}");
            Console.WriteLine($"VnpayLibrary - Sign data: {signData}");
            Console.WriteLine($"VnpayLibrary - Secure hash: {vnpSecureHash}");
            Console.WriteLine($"VnpayLibrary - Final URL: {paymentUrl}");

            return paymentUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            if (string.IsNullOrEmpty(inputHash) || string.IsNullOrEmpty(secretKey))
                return false;

            var rspRaw = GetResponseData();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return string.Equals(myChecksum, inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string HmacSha512(string key, string inputData)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(inputData))
                return string.Empty;

            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA512(keyBytes);
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            _responseData.Remove("vnp_SecureHashType");
            _responseData.Remove("vnp_SecureHash");

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            if (data.Length > 0)
            {
                data.Length--;  // Remove the last '&'
            }

            return data.ToString();
        }
    }
}

namespace Eshopper_website.Libraries
{
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("vi-VN");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}



