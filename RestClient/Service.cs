using BrassLoon.RestClient.Internal;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public class Service : IService
    {
        public void CancelPendingRequests() => HttpClientBuilder.HttpClient.CancelPendingRequests();

        public IRequest CreateRequest(Uri address, HttpMethod method) => new Request(address, method);

        public IRequest CreateRequest(Uri address, HttpMethod method, object body)
        {
            return CreateRequest(address, method)
                .AddJsonBody(body);
        }

        public async Task<IResponse> Delete(Uri uri, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.HttpClient.DeleteAsync(uri, tkn),
                token));
        }

        public async Task<IResponse<T>> Delete<T>(Uri uri, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.HttpClient.DeleteAsync(uri, tkn),
                token));
        }

        public async Task<IResponse> Get(Uri uri, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.HttpClient.GetAsync(uri, tkn),
                token));
        }

        public async Task<IResponse<T>> Get<T>(Uri uri, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.HttpClient.GetAsync(uri, tkn),
                token));
        }

        public async Task<byte[]> GetBytes(Uri uri) => await HttpClientBuilder.HttpClient.GetByteArrayAsync(uri);

        public async Task<Stream> GetStream(Uri uri) => await HttpClientBuilder.HttpClient.GetStreamAsync(uri);

        public async Task<string> GetString(Uri uri) => await HttpClientBuilder.HttpClient.GetStringAsync(uri);

        public async Task<IResponse> Post(Uri uri, object body = null, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn =>
                {
                    using (HttpContent content = JsonRequestContentBuilder.Build(body))
                    {
                        return await HttpClientBuilder.HttpClient.PostAsync(uri, content, tkn);
                    }
                },
                token));
        }

        public async Task<IResponse<T>> Post<T>(Uri uri, object body = null, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn =>
                {
                    using (HttpContent content = JsonRequestContentBuilder.Build(body))
                    {
                        return await HttpClientBuilder.HttpClient.PostAsync(uri, content, tkn);
                    }
                },
                token));
        }

        public async Task<IResponse> Put(Uri uri, object body = null, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn =>
                {
                    using (HttpContent content = JsonRequestContentBuilder.Build(body))
                    {
                        return await HttpClientBuilder.HttpClient.PutAsync(uri, content, tkn);
                    }
                },
                token));
        }

        public async Task<IResponse<T>> Put<T>(Uri uri, object body = null, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn =>
                {
                    using (HttpContent content = JsonRequestContentBuilder.Build(body))
                    {
                        return await HttpClientBuilder.HttpClient.PutAsync(uri, content, tkn);
                    }
                },
                token));
        }

        private static async Task<HttpResponseMessage> GetWebResponseInternal(
            Func<CancellationToken, Task<HttpResponseMessage>> getResponse,
            CancellationToken token = default)
            => await getResponse(token);

        private static async Task<IResponse<T>> CreateResponseInternal<T>(HttpResponseMessage response)
        {
            IResponseFactory factory = new ResponseFactory();
            return await factory.Create<T>(response);
        }

        public async Task<IResponse> Send(IRequest request, CancellationToken token = default)
        {
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                HttpResponseMessage responseMessage = await HttpClientBuilder.HttpClient.SendAsync(requestMessage, token);
                IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                return await responseFactory.Create(responseMessage);
            }
        }

        public async Task<IResponse<T>> Send<T>(IRequest request, CancellationToken token = default)
        {
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                HttpResponseMessage responseMessage = await HttpClientBuilder.HttpClient.SendAsync(requestMessage, token);
                IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                return await responseFactory.Create<T>(responseMessage);
            }
        }
    }
}
