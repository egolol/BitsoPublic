using System.Threading.Tasks;
using Utils.Model;

namespace WebServices.Client
{
    internal static class ManifestException
    {
        public static async Task<ResponseModel> AdministradorExcepciones(RestfulResponse response)
             => new ResponseModel(response.Response == null ? string.Empty :
                                 response.GetHttpStatusCodeType().Equals(HttpStatusType.Successful) ?
                                 await response.ReadResponseAsStringAsync() :
                                 response.GetStatusCode().ToString(),
                                 response.ExceptionMessage);
    }
}
