﻿using System.Net;

namespace OnlineStore.Exceptions
{
    public class RESTException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public RESTException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
