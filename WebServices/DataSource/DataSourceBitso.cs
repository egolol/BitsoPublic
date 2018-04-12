using System.Threading.Tasks;
using Utils.Model;
using WebServices.DataSource;
using static WebServices.Client.GetConfiguration;
using static WebServices.Client.ManifestException;

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

        public static async Task<ResponseModel> RequestSigning(string signature)
            => await AdministradorExcepciones(
                await InvokeBitso.GetRequestPrivateAsync(GetSignatureUrl(UrlApiBitso, Balance)));

        public static async Task<ResponseModel> RequestAccountStatus(string signature)
            => await AdministradorExcepciones(
                await InvokeBitso.GetRequestPrivateAsync(GetSignatureUrl(UrlApiBitso, Status)));

    }
}
