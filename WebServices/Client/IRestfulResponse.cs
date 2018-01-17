using System.Net;
using System.Threading.Tasks;

namespace WebServices.Client
{
    public interface IRestfulResponse
    {
        HttpStatusCode GetStatusCode();
        Task<string> ReadResponseAsStringAsync();
    }
}