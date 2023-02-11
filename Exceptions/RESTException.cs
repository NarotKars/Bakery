using System.Net;

namespace OnlineStore.Exceptions
{
    public class RESTException : Exception
    {
        public RESTException(string message) : base(message)
        {
        }
    }
}
