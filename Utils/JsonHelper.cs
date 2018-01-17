using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Utils
{
    public class JsonHelper
    {
        public static string JsonSerializer<T>(T t)
        {
            var json = string.Empty;

            try
            {
                using (var ms = new MemoryStream())
                {
                    var ser = new DataContractJsonSerializer(typeof(T));
                    ser.WriteObject(ms, t);
                    json = Encoding.UTF8.GetString(ms.ToArray());
                }

                return json;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        public static ResponseJson<T> JsonDeserialize<T>(string jsonString)
        {
            ResponseJson<T> response = null;

            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    var ser = new DataContractJsonSerializer(typeof(T));
                    var obj = (T)ser.ReadObject(ms);
                    response = new ResponseJson<T>(obj, string.Empty);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseJson<T>(ex.Message);
            }
        }
    }
}