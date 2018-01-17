using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Model;
using static WebServices.Client.ManifestException;
using static WebServices.Client.GetConfiguration;
using WebServices.DataSource;

namespace WebServices
{
    public static class DataSourceBitso
    {
        public static async Task<ResponseModel> GetAvailableBookAsync()
       => await AdministradorExcepciones(
          await InvokeBitso.GetRequestPublicAsync(GetUrl(UrlApiBitso, AvailableBooks)));

        public static async Task<ResponseModel> GetTickerAsync()
        => await AdministradorExcepciones(
           await InvokeBitso.GetRequestPublicAsync(GetUrl(UrlApiBitso, Ticker)));

        public static async Task<ResponseModel> GetAOrderBookAsync(string book, bool aggregate)
        => await AdministradorExcepciones(
        await InvokeBitso.GetRequestPublicAsync(GetUrl(UrlApiBitso, OrderBook) + $"book={book}&aggregate={aggregate}"));

        public static async Task<ResponseModel> GetTradesAsync(string book)
         => await AdministradorExcepciones(
         await InvokeBitso.GetRequestPublicAsync(GetUrl(UrlApiBitso, Trades) + $"book={book}&sort=desc&limit=100"));

        public static async Task<ResponseModel> GetTradesAsync(string book, int id, string sort)
            => await AdministradorExcepciones(
             await InvokeBitso.GetRequestPublicAsync(GetUrl(UrlApiBitso, Trades) + $"book={book}&marker={id}&sort={sort}&limit=100"));

    }
}
