using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Internal
{
    public class ResponseFactory : IResponseFactory
    {
        internal ResponseFactory() { }

        public async Task<IResponse> Create(HttpResponseMessage responseMessage)
        {
            string text = null;
            object json = null;
            if (!GetNoMessageContent(responseMessage))
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower(CultureInfo.InvariantCulture))
                {
                    case "text/plain":
                    case "text/csv":
                        text = await CreateText(responseMessage);
                        break;
                    case "application/problem+json":
                        json = await CreateJson(responseMessage);
                        text = CreateJsonToText(json);
                        break;
                    case "application/json":
                        json = await CreateJson(responseMessage);
                        break;
                }
            }
            return new Response(responseMessage)
            {
                Text = text,
                Json = json
            };
        }

        public async Task<IResponse<T>> Create<T>(HttpResponseMessage responseMessage)
        {
            T value = default(T);
            string text = null;
            object json = null;
            if (!GetNoMessageContent(responseMessage))
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower(CultureInfo.InvariantCulture))
                {
                    case "text/plain":
                    case "text/csv":
                        text = await CreateText(responseMessage);
                        if (typeof(string).Equals(typeof(T)))
                            value = (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
                        break;
                    case "application/problem+json":
                        json = await CreateJson(responseMessage);
                        text = CreateJsonToText(json);
                        break;
                    default:
                        CreateJsonResponse<T> createJsonResponse = await CreateJsonOnSuccess<T>(responseMessage);
                        value = createJsonResponse.Value;
                        json = createJsonResponse.Json;
                        break;
                }
            }
            return Create(responseMessage, value, text, json);
        }

        public IResponse<T> Create<T>(HttpResponseMessage responseMessage, T value, string text = null, object json = null)
            => new Response<T>(responseMessage, value)
            {
                Text = text,
                Json = json
            };

        private static bool GetNoMessageContent(HttpResponseMessage responseMessage)
        {
            return (responseMessage.Content.Headers.ContentLength.HasValue
                && responseMessage.Content.Headers.ContentLength.Value == 0L)
                || responseMessage.Content.Headers.ContentType == null
                || responseMessage.StatusCode == HttpStatusCode.NoContent;
        }

        private static async Task<CreateJsonResponse<T>> CreateJsonOnSuccess<T>(HttpResponseMessage responseMessage)
        {
            T value = default(T);
            object json;
            if (responseMessage.IsSuccessStatusCode)
            {
                value = await CreateJson<T>(responseMessage);
                json = value;
            }
            else
            {
                json = await CreateJson(responseMessage);
            }
            return new CreateJsonResponse<T> { Value = value, Json = json };
        }

        private static string CreateJsonToText(object json)
            => JsonConvert.SerializeObject(json, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        private static async Task<T> CreateJson<T>(HttpResponseMessage responseMessage)
        {
            T value = default(T);
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = JsonRequestContentBuilder.Deserialize<T>(outStream);
            }
            return value;
        }

        private static async Task<object> CreateJson(HttpResponseMessage responseMessage)
        {
            object value = null;
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = JsonRequestContentBuilder.Deserialize(outStream);
            }
            return value;
        }

        private static async Task<string> CreateText(HttpResponseMessage responseMessage)
        {
            string value = default(string);
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = TextRequestContentBuilder.Deserialize(outStream);
            }
            return value;
        }

        private struct CreateJsonResponse<T>
        {
            public T Value { get; set; }

            public object Json { get; set; }
        }
    }
}
