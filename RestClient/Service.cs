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
        public void CancelPendingRequests()
        {
            foreach (HttpClient client in HttpClientBuilder.GetEnumerator())
            {
                client.CancelPendingRequests();
            }
        }
        public void ClearCache() => HttpClientBuilder.ClearCache();

        public IRequest CreateRequest(Uri address, HttpMethod method) => new Request(address, method);

        public IRequest CreateRequest(Uri address, TimeSpan timeout, HttpMethod method) => new Request(address, timeout, method);

        public IRequest CreateRequest(Uri address, HttpMethod method, object body)
        {
            return CreateRequest(address, method)
                .AddJsonBody(body);
        }

        public IRequest CreateRequest(Uri address, TimeSpan timeout, HttpMethod method, object body)
        {
            return CreateRequest(address, timeout, method)
                .AddJsonBody(body);
        }

        public async Task<IResponse> Delete(Uri uri, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get().DeleteAsync(uri, tkn),
                token));
        }

        public async Task<IResponse> Delete(Uri uri, TimeSpan timeout, CancellationToken token = default(CancellationToken))
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get(timeout).DeleteAsync(uri, tkn),
                token));
        }

        public async Task<IResponse<T>> Delete<T>(Uri uri, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get().DeleteAsync(uri, tkn),
                token));
        }
        public async Task<IResponse<T>> Delete<T>(Uri uri, TimeSpan timeout, CancellationToken token = default(CancellationToken))
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get(timeout).DeleteAsync(uri, tkn),
                token));
        }

        public async Task<IResponse> Get(Uri uri, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get().GetAsync(uri, tkn),
                token));
        }

        public async Task<IResponse> Get(Uri uri, TimeSpan timeout, CancellationToken token = default(CancellationToken))
        {
            return new Response(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get(timeout).GetAsync(uri, tkn),
                token));
        }

        public async Task<IResponse<T>> Get<T>(Uri uri, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get().GetAsync(uri, tkn),
                token));
        }

        public async Task<IResponse<T>> Get<T>(Uri uri, TimeSpan timeout, CancellationToken token = default(CancellationToken))
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseInternal(async tkn => await HttpClientBuilder.Get(timeout).GetAsync(uri, tkn),
                token));
        }

        public async Task<byte[]> GetBytes(Uri uri) => await HttpClientBuilder.Get().GetByteArrayAsync(uri);

        public async Task<byte[]> GetBytes(Uri uri, TimeSpan timeout) => await HttpClientBuilder.Get(timeout).GetByteArrayAsync(uri);

        public async Task<Stream> GetStream(Uri uri) => await HttpClientBuilder.Get().GetStreamAsync(uri);

        public async Task<Stream> GetStream(Uri uri, TimeSpan timeout) => await HttpClientBuilder.Get(timeout).GetStreamAsync(uri);

        public async Task<string> GetString(Uri uri) => await HttpClientBuilder.Get().GetStringAsync(uri);

        public async Task<string> GetString(Uri uri, TimeSpan timeout) => await HttpClientBuilder.Get(timeout).GetStringAsync(uri);

        public async Task<IResponse> Post(Uri uri, object body = null, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(),
                    (client, u, content, tkn) => client.PostAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse> Post(Uri uri, TimeSpan timeout, object body = null, CancellationToken token = default(CancellationToken))
        {
            return new Response(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(timeout),
                    (client, u, content, tkn) => client.PostAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse<T>> Post<T>(Uri uri, object body = null, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(),
                    (client, u, content, tkn) => client.PostAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse<T>> Post<T>(Uri uri, TimeSpan timeout, object body = null, CancellationToken token = default(CancellationToken))
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(timeout),
                    (client, u, content, tkn) => client.PostAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse> Put(Uri uri, object body = null, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(),
                    (client, u, content, tkn) => client.PutAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse> Put(Uri uri, TimeSpan timeout, object body = null, CancellationToken token = default)
        {
            return new Response(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(timeout),
                    (client, u, content, tkn) => client.PutAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse<T>> Put<T>(Uri uri, object body = null, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(),
                    (client, u, content, tkn) => client.PutAsync(u, content, tkn),
                    token));
        }

        public async Task<IResponse<T>> Put<T>(Uri uri, TimeSpan timeout, object body = null, CancellationToken token = default)
        {
            return await CreateResponseInternal<T>(
                await GetWebResponseWithJsonContentInternal(
                    uri,
                    body,
                    () => HttpClientBuilder.Get(timeout),
                    (client, u, content, tkn) => client.PutAsync(u, content, tkn),
                    token));
        }

        private static async Task<HttpResponseMessage> GetWebResponseInternal(
            Func<CancellationToken, Task<HttpResponseMessage>> getResponse,
            CancellationToken token = default)
            => await getResponse(token);

        private static async Task<HttpResponseMessage> GetWebResponseWithJsonContentInternal(
            Uri uri,
            object body,
            Func<HttpClient> getHttpClient,
            Func<HttpClient, Uri, HttpContent, CancellationToken, Task<HttpResponseMessage>> getResponse,
            CancellationToken token = default)
        {
            HttpClient client = getHttpClient();
            using (HttpContent content = JsonRequestContentBuilder.Build(body))
            {
                return await getResponse(client, uri, content, token);
            }
        }
        private static async Task<IResponse<T>> CreateResponseInternal<T>(HttpResponseMessage response)
        {
            IResponseFactory factory = new ResponseFactory();
            return await factory.Create<T>(response);
        }

        public async Task<IResponse> Send(IRequest request, CancellationToken token = default)
        {
            HttpClient client = HttpClientBuilder.Get(request.Timeout);
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage, token);
                IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                return await responseFactory.Create(responseMessage);
            }
        }

        public async Task<IResponse<T>> Send<T>(IRequest request, CancellationToken token = default)
        {
            HttpClient client = HttpClientBuilder.Get(request.Timeout);
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage, token);
                IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                return await responseFactory.Create<T>(responseMessage);
            }
        }
    }
}
