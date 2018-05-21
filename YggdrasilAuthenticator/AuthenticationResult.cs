using System;
using System.Net;

namespace YggdrasilAuthenticator
{
    public class AuthenticationResult : ErrorResult
    {
        public string AccessToken;
        public string ProfileName;
        public Guid ProfileId;
        public bool IsLegacyProfile;

        public AuthenticationResult(string accessToken, string profileName, Guid profileId, bool isLegacyProfile) : base(HttpStatusCode.OK, null, null, null)
        {
            AccessToken = accessToken;
            ProfileName = profileName;
            ProfileId = profileId;
            IsLegacyProfile = isLegacyProfile;
        }

        public AuthenticationResult(HttpStatusCode statusCode, string error, string errorMessage, string cause) : base(statusCode, error, errorMessage, cause) { }
    }
}
