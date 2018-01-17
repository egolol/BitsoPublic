using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebServices.Client
{
    public class RestfulResponse 
    {
        public HttpWebResponse Response = null;
        public string ExceptionMessage = string.Empty;

       public HttpStatusCode GetStatusCode()
         => this.Response.StatusCode;

       public async Task<string> ReadResponseAsStringAsync()
        => await new StreamReader(this.Response.GetResponseStream()).ReadToEndAsync();

        public bool IsSuccesStatusCode()
        => (int)this.Response.StatusCode >= 200 && (int)this.Response.StatusCode < 300;


        public bool IsRedirectedStatusCode()
           => (int)this.Response.StatusCode >= 300 && (int)this.Response.StatusCode < 400;


        public bool IsErrorStatusCode()
            => (int)this.Response.StatusCode >= 400 && (int)this.Response.StatusCode < 500;


        public bool IsFaultStatusCode()
            => (int)this.Response.StatusCode >= 500 && (int)this.Response.StatusCode < 600;

        public HttpStatusType GetHttpStatusCodeType()
        {
            HttpStatusType result = HttpStatusType.Successful;

            if (this.IsRedirectedStatusCode())
                result = HttpStatusType.Redirected;
            if (this.IsErrorStatusCode())
                result = HttpStatusType.Error;
            if (this.IsFaultStatusCode())
                result = HttpStatusType.Fault;

            return result;
        }

        public RestfulResponse(string message)
        {
            this.Response = null;
            this.ExceptionMessage = message;
        }

        public RestfulResponse(HttpWebResponse response)
        {
            this.Response = response;
        }

        public RestfulResponse(HttpWebResponse response, string message)
        {
            this.Response = response;
            this.ExceptionMessage = message;
        }

    }
    public enum HttpStatusType
    {
        /// <summary>
        /// 2xx
        /// </summary>
        Successful,
        /// <summary>
        /// 3xx
        /// </summary>
        Redirected,
        /// <summary>
        /// 4xx
        /// </summary>
        Error,
        /// <summary>
        /// 5xxx
        /// </summary>
        Fault
    }
}