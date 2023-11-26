using System.Net;

namespace LandisGyrProject
{
    public class DefaultResult<T>
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public T content { get; set; }
    }
}
