﻿using System;
using System.Net;

namespace YggdrasilAuthenticator
{
    public class ValidationResult : ErrorResult
    {
        public bool IsValid;

        public ValidationResult(bool isValid) : base(HttpStatusCode.NoContent, null, null, null)
        {
            IsValid = isValid;
        }

        public ValidationResult(HttpStatusCode statusCode, string error, string errorMessage, string cause) : base(statusCode, error, errorMessage, cause) { }
    }
}
