using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace AT.Github.Automation.Framework
{
    public static class ResponseExtension
    {
        public static dynamic Body(this IRestResponse obj) => JsonConvert.DeserializeObject(obj.Content);

        public static T DeserializeTo<T>(this IRestResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
                throw new NullReferenceException(nameof(response.Content));
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public static T CheckStatusCodeAndDeserializeTo<T>(this IRestResponse response, HttpStatusCode statusCode)
        {
            if (response.StatusCode != statusCode)
                throw new HttpException($"Unexpected http status code: {response.StatusCode}. Expected: {statusCode}");
            return response.DeserializeTo<T>();
        }

        public static Dictionary<string, string> Errors(this IRestResponse response)
            => response.DeserializeTo<Dictionary<string, string>>();
    }
}