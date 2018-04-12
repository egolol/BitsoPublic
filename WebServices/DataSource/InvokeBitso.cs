using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebServices.Client;

namespace WebServices.DataSource
{
    internal static class InvokeBitso
    {
        public static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public static async Task<RestfulResponse> GetRequestPublicAsync(string url)
        {
            if (url == null)
                return new RestfulResponse(null, "No se ha encontrado relación entre propiedad y valor en: AppConfig");

            await semaphore.WaitAsync();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                return new RestfulResponse((HttpWebResponse)(await request.GetResponseAsync()));
            }
            catch (Exception ex)
            {
                var message = ex.Message + " " + ex.InnerException?.Message;

                return new RestfulResponse(message);
            }
            finally
            {
                semaphore.Release();
            }

        }

        public static async Task<RestfulResponse> GetRequestPrivateAsync(string[] info)
        {
            string url = info[0];
            if (url == null)
                return new RestfulResponse(null, "No se puede conectar al servicio de cuenta");
            await semaphore.WaitAsync();

            string authHeader = String.Format($"Bitso {0}:{1}:{2}", info[1], info[2], info[3]);

            HttpWebRequest request =(HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", authHeader);  ///???????

            try
            {
                return new RestfulResponse((HttpWebResponse)(await request.GetResponseAsync()));
            }
            catch (Exception ex)
            {
                var message = ex.Message + " " + ex.InnerException?.Message;
                return new RestfulResponse(message);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}