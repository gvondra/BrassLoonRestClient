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
            string text = null;
            if (responseMessage.Content.Headers.ContentLength.HasValue 
                && responseMessage.Content.Headers.ContentLength.Value > 0L 
                && responseMessage.Content.Headers.ContentType != null)
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower())
                {
                    case "text/plain":
                        text = await CreateText(responseMessage);
                        if (typeof(string).Equals(typeof(T)))
                            value = (T)Convert.ChangeType(text, typeof(T));
                        break;
                    default:
                        value = await CreateJson<T>(responseMessage);
                        break;
                }
            }
            return new Response<T>(responseMessage, value)
            { 
                Text = text
            };
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

        private async Task<string> CreateText(HttpResponseMessage responseMessage)
        {
            string value = default(string);
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = TextRequestContentBuilder.Deserialize(outStream);
            }
            return value;
        }
    }
}
