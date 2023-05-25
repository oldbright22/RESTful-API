using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAPI.Response;

namespace TestAPI.Auth
{
    public class APIAuthenticator : AuthenticatorBase
    {

        readonly string baseUrl;
        readonly string clientId;
        readonly string clientSecret;

        public APIAuthenticator() : base("")
        {

        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            var token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;

            return new HeaderParameter(KnownHeaders.Authorization, token);
        }


        private async Task<string> GetToken()
        {
            var options = new RestClientOptions(baseUrl);
            options.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);

            //https:\\restsharp.dev/authenticators.html#basic-authentication
            //options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");

            var client = new RestClient(options);

            var request = new RestRequest("oauth2/token")
                .AddParameter("grant_type", "client_credentials");

            var response = await client.PostAsync<TokenResponse>(request);

            return $"{response.TokenType} {response.AccessToken}";
        }

    }
}


/*
 * 
 * 
 *       private const string LoginEndpoint = "/rest/auth/1/session";

        [TestMethod]
        public void TestJiraLogin()
        {
            JiraLogin jiraLogin = new JiraLogin()
            {
                username = "rahul",
                password = "admin@1234#"
            };

            IRestClient client = new RestClient()
            {
                BaseUrl = new Uri("http://localhost:9191")
            };

            IRestRequest request = new RestRequest()
            {
                Resource = LoginEndpoint
            };
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(jiraLogin);
            request.AddHeader("Contenet-Type", "application/json");

            var response = client.Post<LoginResponse>(request);
            Console.WriteLine(response.Data);

*/
