using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
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
            if (responseMessage.Content.Headers.ContentLength.HasValue
                && responseMessage.Content.Headers.ContentLength.Value > 0L
                && responseMessage.Content.Headers.ContentType != null)
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower())
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
            if (responseMessage.Content.Headers.ContentLength.HasValue 
                && responseMessage.Content.Headers.ContentLength.Value > 0L 
                && responseMessage.Content.Headers.ContentType != null)
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower())
                {
                    case "text/plain":
                    case "text/csv":
                        text = await CreateText(responseMessage);
                        if (typeof(string).Equals(typeof(T)))
                            value = (T)Convert.ChangeType(text, typeof(T));
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

        private async Task<CreateJsonResponse<T>> CreateJsonOnSuccess<T>(HttpResponseMessage responseMessage)
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

        private async Task<T> CreateJson<T>(HttpResponseMessage responseMessage)
        {
            T value = default(T);
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = JsonRequestContentBuilder.Deserialize<T>(outStream);
            }
            return value;
        }

        private string CreateJsonToText(object json)
        => JsonConvert.SerializeObject(json, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

        private async Task<object> CreateJson(HttpResponseMessage responseMessage)
        {
            object value = null;
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = JsonRequestContentBuilder.Deserialize(outStream);
            }
            return value;
        }

        private async Task<string> CreateText(HttpResponseMessage responseMessage)
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
