using System;
using System.Collections.Concurrent;
using System.Configuration;

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


        public static string GetUrl(string url, string property)
        {
            var _url = GetValue(url);

            var _property = GetValue(property);

            if (_url == null || _property == null)
                return null;

            return _url + _property;

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
    }
}