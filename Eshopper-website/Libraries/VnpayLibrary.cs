using Eshopper_website.Models.VNPay;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace Eshopper_website.Libraries
{
    public class VnpayLibrary
    {
        private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}&");
                }
            }

            string signData = data.ToString().TrimEnd('&');
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

            data.Append($"vnp_SecureHash={vnp_SecureHash}");

            string paymentUrl = baseUrl + "?" + data.ToString();
            
            // Log for debugging
            Console.WriteLine($"Raw Data: {signData}");
            Console.WriteLine($"Hash Secret: {vnp_HashSecret}");
            Console.WriteLine($"Secure Hash: {vnp_SecureHash}");
            Console.WriteLine($"Payment URL: {paymentUrl}");

            return paymentUrl;
        }

        public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var vnpayData = new PaymentResponseModel
            {
                Success = false,
                OrderDescription = string.Empty,
                TransactionId = string.Empty,
                OrderId = string.Empty,
                PaymentMethod = "VNPay",
                PaymentId = string.Empty,
                Token = string.Empty,
                VnPayResponseCode = string.Empty,
                Message = string.Empty,
                Amount = 0
            };

            if (collection.Count > 0)
            {
                // Get the secure hash from the response
                var secureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value.ToString();

                // Add all fields except secure hash to the response data
                foreach (var (key, value) in collection)
                {
                    if (key != "vnp_SecureHash" && !string.IsNullOrEmpty(value))
                    {
                        _responseData[key] = value.ToString();
                    }
                }

                // Validate the signature
                bool checkSignature = ValidateSignature(secureHash, hashSecret);
                if (!checkSignature)
                {
                    vnpayData.Message = "Invalid Signature";
                    return vnpayData;
                }

                vnpayData.Success = true;
                vnpayData.OrderId = collection.FirstOrDefault(k => k.Key == "vnp_TxnRef").Value.ToString();
                vnpayData.VnPayResponseCode = collection.FirstOrDefault(k => k.Key == "vnp_ResponseCode").Value.ToString();
                vnpayData.OrderDescription = collection.FirstOrDefault(k => k.Key == "vnp_OrderInfo").Value.ToString();
                
                string amountStr = collection.FirstOrDefault(k => k.Key == "vnp_Amount").Value.ToString();
                if (long.TryParse(amountStr, out long amount))
                {
                    vnpayData.Amount = amount / 100M;
                }

                vnpayData.PaymentId = collection.FirstOrDefault(k => k.Key == "vnp_TransactionNo").Value.ToString();
                vnpayData.TransactionId = vnpayData.PaymentId;
                vnpayData.Token = secureHash;
                vnpayData.Message = "Success";
            }

            return vnpayData;
        }

        public string GetResponseData()
        {
            StringBuilder data = new StringBuilder();
            
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}&");
                }
            }

            // Remove last '&' character
            if (data.Length > 0)
            {
                data.Length--;
            }

            return data.ToString();
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString().ToLower();
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = HmacSHA512(secretKey, rspRaw);

            // Log for debugging
            Console.WriteLine($"Response Data: {rspRaw}");
            Console.WriteLine($"Secret Key: {secretKey}");
            Console.WriteLine($"Generated Hash: {myChecksum}");
            Console.WriteLine($"Input Hash: {inputHash}");

            return string.Equals(myChecksum, inputHash, StringComparison.OrdinalIgnoreCase);
        }

        public string GetIpAddress(HttpContext context)
        {
            string ipAddress;
            try
            {
                ipAddress = context.Connection.RemoteIpAddress?.ToString();
                if (string.IsNullOrEmpty(ipAddress) || ipAddress.Equals("::1", StringComparison.OrdinalIgnoreCase))
                {
                    ipAddress = "127.0.0.1";
                }
            }
            catch (Exception)
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");

            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}



