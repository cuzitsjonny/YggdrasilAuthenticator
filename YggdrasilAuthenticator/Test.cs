using System;
using System.IO;

namespace YggdrasilAuthenticator
{
    class Test
    {
        static void Main(string[] args)
        {
            // First of all we need to instantiate an Authenticator.
            // We can instantiate it without a client token (GUID), but it's better
            // to generate one and use it over and over again.
            // If no client token is provided, the Authenticator will
            // generate one itself automatically.
            // You can get it using Authenticator::GetClientToken.
            //
            // This is probably the best way to go:
            // Guid clientToken = Guid.NewGuid(); // generate it, and save it
            // Authenticator auth = new Authenticator(clientToken); // use it
            Authenticator auth = new Authenticator();
            
            // Username/email address and password are self explanatory
            string usernameOrEmailAddress = "123@test.test";
            string password = "password";

            // We'll get an access token from a succssful authentication.
            string accessToken = null;

            // As you can see, an authentication is pretty easy. You can use the Authenticator for Minecraft and Scrolls btw.
            AuthenticationResult authResult = auth.Authenticate(usernameOrEmailAddress, password, AgentType.Minecraft, 1);

            // Let's check if an error occured.
            if(authResult.Error == null)
            {
                // No error, so there is our access token.
                accessToken = authResult.AccessToken;

                Console.WriteLine("Successfully acquired access token: {0}", accessToken); 
            }
            else
            {
                // An error is contructed from an error title (Error), and error message (ErrorMessage), and a cause (Cause).
                // You will always receive and error title, and an error message, but not always a cause.
                if(authResult.Cause == null)
                {
                    Console.WriteLine("{0}: {1}", authResult.Error, authResult.ErrorMessage);
                }
                else
                {
                    Console.WriteLine("{0}: {1} ({2})", authResult.Error, authResult.ErrorMessage, authResult.Cause);
                }
            }

            // If the authentication earlier was unsuccessful, our access token is null now.
            // So better catch that possibility here.
            if(accessToken != null)
            {
                // We can validate if our access token is still valid for playing using Authenticator::Validate.
                ValidationResult valResult = auth.Validate(accessToken);

                if(valResult.Error == null) // or valResult.IsValid
                {
                    Console.WriteLine("Validated access token. It's still valid!");
                }
            }

            if (accessToken != null)
            {
                // We can refresh an access token using Authenticator::Refresh.
                // Sometimes an access token is not valid for playing anymore, but you can still refresh it.
                // Always check if you can refresh an access token before requesting a new one.
                RefreshResult refResult = auth.Refresh(accessToken);

                if (refResult.Error == null)
                {
                    Console.WriteLine("Refreshed access token: {0}", refResult.NewAccessToken);
                }
            }

            if (accessToken != null)
            {
                // We can invalidate an access token requested using our client token using Authenticator::Invalidate.
                InvalidationResult invResult = auth.Invalidate(accessToken);

                if (invResult.Error == null) // or invResult.IsSuccessful
                {
                    Console.WriteLine("Invalidated access token.");
                }
            }

            // We can invalidate all access tokens of a certain account using Authenticator::Signout.
            InvalidationResult sigResult = auth.Signout(usernameOrEmailAddress, password);

            if (sigResult.Error == null) // or invResult.IsSuccessful
            {
                Console.WriteLine("Invalidated all access tokens for this account.");
            }

            // We can also use the client token the Authenticator generated and save it for later use once we're done.
            // File.WriteAllText("/path/to/file", auth.GetClientToken());
        }
    }
}
