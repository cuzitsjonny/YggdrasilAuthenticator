using System;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace YggdrasilAuthenticator
{
    public class Authenticator
    {
        private HttpClient _Client;
        private Guid _ClientToken;

        public Authenticator(Guid clientToken)
        {
            _Client = new HttpClient();
            _ClientToken = clientToken;
        }

        public Authenticator() : this(Guid.NewGuid()) { }

        public Guid GetClientToken()
        {
            return _ClientToken;
        }

        public AuthenticationResult Authenticate(string usernameOrEmailAddress, string password, AgentType agentType, int agentVersion)
        {
            AuthenticationResult result = null;

            JObject payload = new JObject();
            JObject agent = new JObject();

            agent["name"] = (agentType == AgentType.Minecraft ? "Minecraft" : "Scrolls");
            agent["version"] = agentVersion;

            payload["agent"] = agent;
            payload["username"] = usernameOrEmailAddress;
            payload["password"] = password;
            payload["clientToken"] = _ClientToken.ToString();

            HttpContent content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = _Client.PostAsync("https://authserver.mojang.com/authenticate", content).Result;
            JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);

            if(httpResponse.StatusCode == HttpStatusCode.OK)
            {
                JObject selectedProfile = (JObject)response["selectedProfile"];

                string profileName = (string)selectedProfile["name"];
                Guid profileId = Guid.Parse((string)selectedProfile["id"]);
                bool isLegacyProfile = false;

                if(selectedProfile["legacy"] != null)
                {
                    isLegacyProfile = (bool)selectedProfile["legacy"];
                }

                string accessToken = response["accessToken"].ToString();

                result = new AuthenticationResult(accessToken, profileName, profileId, isLegacyProfile);
            }
            else
            {
                string error = (string)response["error"];
                string errorMessage = (string)response["errorMessage"];
                string cause = null;
                
                if(response["cause"] != null)
                {
                    cause = (string)response["cause"];
                }

                result = new AuthenticationResult(httpResponse.StatusCode, error, errorMessage, cause);
            }

            return result;
        }

        public RefreshResult Refresh(string accessToken)
        {
            RefreshResult result = null;

            JObject payload = new JObject();

            payload["accessToken"] = accessToken;
            payload["clientToken"] = _ClientToken.ToString();

            HttpContent content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = _Client.PostAsync("https://authserver.mojang.com/refresh", content).Result;
            JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                string newAccessToken = response["accessToken"].ToString();

                result = new RefreshResult(newAccessToken);
            }
            else
            {
                string error = (string)response["error"];
                string errorMessage = (string)response["errorMessage"];
                string cause = null;

                if (response["cause"] != null)
                {
                    cause = (string)response["cause"];
                }

                result = new RefreshResult(httpResponse.StatusCode, error, errorMessage, cause);
            }

            return result;
        }

        public ValidationResult Validate(string accessToken)
        {
            ValidationResult result = null;

            JObject payload = new JObject();

            payload["accessToken"] = accessToken;
            payload["clientToken"] = _ClientToken.ToString();

            HttpContent content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = _Client.PostAsync("https://authserver.mojang.com/validate", content).Result;

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                result = new ValidationResult(true);
            }
            else
            {
                JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);

                string error = (string)response["error"];
                string errorMessage = (string)response["errorMessage"];
                string cause = null;

                if (response["cause"] != null)
                {
                    cause = (string)response["cause"];
                }

                result = new ValidationResult(httpResponse.StatusCode, error, errorMessage, cause);
            }

            return result;
        }

        public InvalidationResult Signout(string usernameOrEmailAddress, string password)
        {
            InvalidationResult result = null;

            JObject payload = new JObject();

            payload["username"] = usernameOrEmailAddress;
            payload["password"] = password;

            HttpContent content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = _Client.PostAsync("https://authserver.mojang.com/signout", content).Result;

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                result = new InvalidationResult(true);
            }
            else
            {
                JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);

                string error = (string)response["error"];
                string errorMessage = (string)response["errorMessage"];
                string cause = null;

                if (response["cause"] != null)
                {
                    cause = (string)response["cause"];
                }

                result = new InvalidationResult(httpResponse.StatusCode, error, errorMessage, cause);
            }

            return result;
        }

        public InvalidationResult Invalidate(string accessToken)
        {
            InvalidationResult result = null;

            JObject payload = new JObject();

            payload["accessToken"] = accessToken;
            payload["clientToken"] = _ClientToken.ToString();

            HttpContent content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = _Client.PostAsync("https://authserver.mojang.com/invalidate", content).Result;

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                result = new InvalidationResult(true);
            }
            else
            {
                JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);

                string error = (string)response["error"];
                string errorMessage = (string)response["errorMessage"];
                string cause = null;

                if (response["cause"] != null)
                {
                    cause = (string)response["cause"];
                }

                result = new InvalidationResult(httpResponse.StatusCode, error, errorMessage, cause);
            }

            return result;
        }
    }
}
