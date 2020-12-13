using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Internal
{
    public class ResponseFactory : IResponseFactory
    {
        internal ResponseFactory() { }

        public Task<IResponse> Create(HttpResponseMessage responseMessage)
        {
            return Task.FromResult<IResponse>(new Response(responseMessage));
        }

        public async Task<IResponse<T>> Create<T>(HttpResponseMessage responseMessage)
        {            
            T value = default(T);
            switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower())
            {
                case "text/plain":
                    value = await CreateText<T>(responseMessage);
                    break;
                default:
                    value = await CreateJson<T>(responseMessage);
                    break;
            }
            return new Response<T>(responseMessage, value);
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

        private async Task<T> CreateText<T>(HttpResponseMessage responseMessage)
        {
            if (!typeof(string).Equals(typeof(T)))
            {
                throw new ApplicationException($"Invalid return type {typeof(T).Name} for text web response");
            }
            T value = default(T);
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = (T)Convert.ChangeType(TextRequestContentBuilder.Deserialize(outStream), typeof(T));
            }
            return value;
        }
    }
}
