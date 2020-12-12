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
            using (Stream outStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                value = JsonRequestContentBuilder.Deserialize<T>(outStream);
            }
            return new Response<T>(responseMessage, value);
        }
    }
}
