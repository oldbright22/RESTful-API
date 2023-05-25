using Microsoft.AspNetCore.JsonPatch;
using LibraryTestAPI;
using RestSharp;

namespace TestAPI
{
    public interface IAPIClient
    {
        Task<RestResponse> POST<T>(string endpoint, T payload) where T : class;
        Task<RestResponse> PUT<T>(string endpoint, T payload, string id) where T : class;
        Task<RestResponse> PATCH<T>(string endpoint, IEnumerable<Operation> payload, string id) where T : class;
        Task<RestResponse> DELETE<T>(string endpoint, string id) where T : class;
        Task<RestResponse> GETById<T>(string endpoint, string id) where T : class;
        Task<RestResponse> GETList<T>(string endpoint) where T : class; //int pageNumber

    }
}