using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http.Headers;
using NUnit.Framework.Interfaces;
using System.IO;
using LibraryTestAPI;

namespace TestAPI
{
    public class APIClient : IAPIClient, IDisposable
    {
        readonly RestClient restClient;
        public string BASE_URL = "https://regres.in";
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;


        public APIClient()
        {
            var options = new RestClientOptions(BASE_URL);
             restClient = new RestClient(options);
            //options.Authenticator = new APIAuthenticator();

            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                return settings;
            });

        }

        /*
         * 
        private const string AuthorizeUrl = "..../oauth2/authorize";
        private const string GenerateTokenUrl = "...../oauth2/token";
        
        client.Authenticator = new HttpBasicAuthenticator("keg8sdb86l4kdlp", "kusfz7e4twb60k4");
        IRestRequest request = new RestRequest()
        {
            Resource = GenerateTokenUrl
        };

        request.AddParameter("code", "authorization_code", ParameterType.QueryString);
        request.AddParameter("grant_type", "authorization_code", ParameterType.QueryString);
        */


        //Yes response body
        public async Task<RestResponse> POST<T>(string endpoint, T payload) where T : class
        {
            //request.AddHeader("Authorization", "Bearer " + AccessToken);

            var request = new RestRequest(BASE_URL + endpoint, Method.Post);
            request.AddHeader("Accept", "application/json");
            //request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(payload);
            return await restClient.ExecuteAsync<T>(request);
        }

        
        public async Task<RestResponse> PUT<T>(string endpoint, T payload, string id) where T : class
        {
            var request = new RestRequest(BASE_URL + endpoint, Method.Put);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddBody(payload);
            return await restClient.ExecuteAsync<T>(request);
        }


        public async Task<RestResponse> PATCH<T>(string endpoint, IEnumerable<Operation> payload, string id) where T : class
        {
            var request = new RestRequest(BASE_URL + endpoint, Method.Patch);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            //request.AddHeader("Accept", "application/json");
            //request.RequestFormat = DataFormat.Json;
            var content = new StringContent(JsonConvert.SerializeObject(payload,_settings.Value), Encoding.UTF8, "application/json-patch+json");
            request.AddBody(content);

            return await restClient.ExecuteAsync(request);
        }

        //No response body
        public async Task<RestResponse> DELETE<T>(string endpoint, string id) where T : class
        {
            var request = new RestRequest(BASE_URL + endpoint, Method.Delete);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return await restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse> GETById<T>(string endpoint, string id) where T : class
        {
            var request = new RestRequest(BASE_URL + endpoint, Method.Get);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return await restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse> GETList<T>(string endpoint) where T : class
        {
            var request = new RestRequest(BASE_URL + endpoint, Method.Get);
            request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;
            //request.AddQueryParameter("page", pageNumber);
            return await restClient.ExecuteAsync<T>(request);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            //Release resources
            restClient?.Dispose();

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        
    }
}




/*
 * using (var client = new HttpClient())
{

    client.BaseAddress = hostUri;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
    var method = "PATCH";
    var httpVerb = new HttpMethod(method);

    var httpRequestMessage =
        new HttpRequestMessage(httpVerb, path)
        {
            Content = stringContent
        };

    try
    {
        var response = await client.SendAsync(httpRequestMessage);
        if (!response.IsSuccessStatusCode)
        {
            var responseCode = response.StatusCode;
            var responseJson = await response.Content.ReadAsStringAsync();
            throw new MyCustomException($"Unexpected http response {responseCode}: {responseJson}");
        }
    }
    catch (Exception exception)
    {
        throw new MyCustomException($"Error patching {stringContent} in {path}", exception);
    }

}



            //restClient.PatchAsync<T>(endpoint, content);
            //PatchAsync($"v1/Entry/1", content);


            //var uri = Path.Combine("books", "1");
            //var request = new HttpRequestMessage(HttpMethod.Patch, uri);
            //var serializedDoc = JsonConvert.SerializeObject(payload);
            //var requestContent = new StringContent(serializedDoc, Encoding.UTF8);
            //requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

            //return await restClient.ExecuteAsync(request);

            //var uri = Path.Combine("books", "1");
            //var serializedDoc = JsonConvert.SerializeObject(payload);
            //var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
            //var response = await restClient.PatchAsync(uri, requestContent);

            //var requestContent = new StringContent(serializedDoc, Encoding.UTF8);
            //requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

            //new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
            //request.AddParameter("patchDTO", requestContent, ParameterType.RequestBody);

            //return await restClient.ExecuteAsync(request);


*/