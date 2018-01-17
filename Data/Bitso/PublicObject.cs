using Model.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;
using WebServices;

namespace Data.Bitso
{
    public class PublicObject
    {
        async public static Task<List<AvailableBook>> GetAvailableBooksAsync()
        {
            PayLoadObject respuesta = null;
            var getResponse = await DataSourceBitso.GetAvailableBookAsync();
            respuesta = ServiceUtils.GetResponse<PayLoadObject>(getResponse);
            return respuesta.payload;
        }

        async public static Task<List<Ticker>> GetTickersAsync()
        {
            TickerObject respuesta = null;

            var getResponse = await DataSourceBitso.GetTickerAsync();

            respuesta = ServiceUtils.GetResponse<TickerObject>(getResponse);

            return respuesta.payload;

        }

        async public static Task<Orders> GetOrderBookAsync(string book, bool aggregate)
        {
            OrderObject respuesta = null;

            var getResponse = await DataSourceBitso.GetAOrderBookAsync(book, aggregate);

            respuesta = ServiceUtils.GetResponse<OrderObject>(getResponse);

            return respuesta.payload;

        }

        async public static Task<List<Trades>> GetTradesAsync(string book)
        {
            TradesObject respuesta = null;

            var getResponse = await DataSourceBitso.GetTradesAsync(book);

            respuesta = ServiceUtils.GetResponse<TradesObject>(getResponse);

            return respuesta.payload;

        }

        async public static Task<List<Trades>> GetTradesAsync(string book, int id, string sort)
        {

            TradesObject respuesta = null;

            var getResponse = await DataSourceBitso.GetTradesAsync(book, id, sort);
            if (getResponse.Json == string.Empty)
                return null;

            respuesta = ServiceUtils.GetResponse<TradesObject>(getResponse);

            return respuesta.payload;

        }
    }
}
