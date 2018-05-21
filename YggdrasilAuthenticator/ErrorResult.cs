using System;
using System.Net;

namespace YggdrasilAuthenticator
{
    public class ErrorResult
    {
        public HttpStatusCode StatusCode;
        public string Error;
        public string ErrorMessage;
        public string Cause;

        public ErrorResult(HttpStatusCode statusCode, string error, string errorMessage, string cause)
        {
            StatusCode = statusCode;
            Error = error;
            ErrorMessage = errorMessage;
            Cause = cause;
        }
    }
}
