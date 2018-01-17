namespace Utils
{
    public class ResponseJson<T>
    {
        private string exception;

        private T data;

        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        public ResponseJson(T t, string ex)
        {
            this.Data = t;
            this.Exception = ex;
        }

        public ResponseJson(string ex)
        {
            this.Exception = ex;
        }
    }
}