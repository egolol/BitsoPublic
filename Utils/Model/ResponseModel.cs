namespace Utils.Model
{
    public class ResponseModel
    {
        public string Json { get; set; }

        public string Exception { get; set; }

        public ResponseModel(string json, string ex)
        {
            this.Json = json;
            this.Exception = ex;
        }
    }
}