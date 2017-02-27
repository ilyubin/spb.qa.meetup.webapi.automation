using System;
using System.Net;
using AT.Github.Automation.Tests;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using Serilog.Context;

namespace AT.Github.Automation.Framework
{
    public class HttpClient
    {
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public HttpClient(string apiUrl, string apiKey = null)
        {
            if (string.IsNullOrEmpty(apiUrl)) throw new ArgumentNullException(nameof(apiUrl));
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

        public T Post<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Post(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T Patch<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Patch(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T Get<T>(string resource, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Get(resource).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T Delete<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Delete(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T Put<T>(string resource, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Put(resource).CheckStatusCodeAndDeserializeTo<T>(statusCode);

        public IRestResponse Post(string resource, object data = null) =>
            SendRequest(resource, data);
        public IRestResponse Patch(string resource, object data = null) =>
            SendRequest(resource, data, Method.PATCH);
        public IRestResponse Get(string resource) =>
            SendRequest(resource, method: Method.GET);
        public IRestResponse Delete(string resource, object data = null) =>
            SendRequest(resource, data, Method.DELETE);
        public IRestResponse Put(string resource) =>
            SendRequest(resource, method: Method.PUT);

        public T PostAuth<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            PostAuth(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T PatchAuth<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            PatchAuth(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T GetAuth<T>(string resource, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            GetAuth(resource).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T DeleteAuth<T>(string resource, object data = null, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            DeleteAuth(resource, data).CheckStatusCodeAndDeserializeTo<T>(statusCode);
        public T PutAuth<T>(string resource, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            PutAuth(resource).CheckStatusCodeAndDeserializeTo<T>(statusCode);

        public IRestResponse PostAuth(string resource, object data = null) =>
            SendRequest(resource, data, auth: true);
        public IRestResponse PatchAuth(string resource, object data = null) =>
            SendRequest(resource, data, Method.PATCH, true);
        public IRestResponse GetAuth(string resource) =>
            SendRequest(resource, method: Method.GET, auth: true);
        public IRestResponse DeleteAuth(string resource, object data = null) =>
            SendRequest(resource, data, Method.DELETE, true);
        public IRestResponse PutAuth(string resource) =>
            SendRequest(resource, method: Method.PUT, auth: true);

        private IRestResponse SendRequest(string resource, object data = null, Method method = Method.POST, bool auth = false)
        {
            var baseUrl = new Uri(_apiUrl);
            var client = new RestClient(baseUrl);

            var request = GetRestRequest(resource, method);
            var url = new Uri(baseUrl, resource);

            if (auth)
                request.AddHeader("Authorization", "token " + FixtureBase.Configuration["api:github:token"]);

            var requestId = Guid.NewGuid().ToString("N");
            IRestResponse response;
            using (LogContext.PushProperty("RequestId", requestId, true))
            {
                if (data != null)
                {
                    var serializeObject = SerializeObject(data);
                    request.AddParameter("application/json", serializeObject, ParameterType.RequestBody);
                    using (LogContext.PushProperty("RequestObject", data, true))
                        Log.Information("req|{Method}|{Url}|{RequestString}", method, url, serializeObject);
                }
                else
                {
                    Log.Information("req|{Method}|{Url}", method, url);
                }
                response = client.Execute(request);
                using (LogContext.PushProperty("ResponseObject", response.Body(), true))
                    Log.Information("res|{StatusCode}|{ResponseString}", response.StatusCode, response.Content);
            }
            return response;
        }

        private static string SerializeObject(object data)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var serializeObject = JsonConvert.SerializeObject(data, settings);
            return serializeObject;
        }

        private RestRequest GetRestRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            if (!string.IsNullOrEmpty(_apiKey)) request.AddHeader("ApiKey", _apiKey);

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return request;
        }
    }
}
