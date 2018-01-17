using Utils.Model;

namespace Utils
{
    public class ServiceUtils
    {
        public static T GetResponse<T>(ResponseModel getResponse)
        {
            return JsonHelper.JsonDeserialize<T>(getResponse.Json).Data;
        }
    }
}