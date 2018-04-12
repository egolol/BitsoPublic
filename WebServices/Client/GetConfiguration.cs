using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebServices.Client
{
    public static class GetConfiguration
    {
        private static ConcurrentDictionary<string, string> Config = new ConcurrentDictionary<string, string>();

        public static string UrlApiBitso = "UrlBitso";
        public static string AvailableBooks = "AvailableBooks";
        public static string Ticker = "Ticker";
        public static string OrderBook = "OrderBook";
        public static string Trades = "Trades";
        public static string Balance = "Balance";
        public static string ApiKey = "ApiKey";
        public static string Status = "Status";

        public static string nonce;

        public static string GetUrl(string url, string property)
        {
            var _url = GetValue(url);

            var _property = GetValue(property);

            if (_url == null || _property == null)
                return null;

            return _url + _property;

        }

        public static string[] GetSignatureUrl(string url, string property)
        {
            var _url = GetValue(url) + GetValue(property);            

            var _info = File.ReadAllLines(GetValue(ApiKey));
            string _key = _info[0];
            string _secret = _info[1];

            nonce= (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();//unix timstamp
            string _httpMethod = "GET";
            string _requestPath = "/v3" + GetValue(property);
            string _jsonPayLoad = "";

            string _signature = CreateSignature(_secret, nonce, _httpMethod, _requestPath, _jsonPayLoad );
            string[] info = new string[4];

            info[0] = _url;
            info[1] = _key;
            info[2] = nonce;
            info[3] = _signature;

            return info;
        }

        private static string GetValue(string property)
        {
            if (Config.ContainsKey(property))
            {
                return Config[property];
            }
            var value = GetAppConfig(property);

            if (value != null)
                Config.TryAdd(property, value);

            return value;

        }

        private static string GetAppConfig(string value)
        {
            try
            {
                return ConfigurationManager.AppSettings[value];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string CreateSignature(string _secret, string _nonce, string _httpMethod, string _requestPath, string _jsonPayload)
        {
            string signNotHashed = _nonce+_httpMethod+_requestPath+_jsonPayload;
            var message = Encoding.UTF8.GetBytes(signNotHashed);
            var secret = Encoding.UTF8.GetBytes(_secret);

            string signature;
            using (var hmac = new HMACSHA256(secret))
            {
                var hash = hmac.ComputeHash(message);
                signature = Convert.ToBase64String(hash);
            }

                return signature;
        }
    }
}