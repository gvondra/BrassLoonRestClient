using BrassLoon.RestClient.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public class Service : IService
    {        
        public void CancelPendingRequests()
        {
            HttpClientBuilder.HttpClient.CancelPendingRequests();
        }

        public IRequest CreateRequest(Uri address, HttpMethod method)
        {
            return new Request(address, method);
        }

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

        public async Task<byte[]> GetBytes(Uri uri)
        {
            return await HttpClientBuilder.HttpClient.GetByteArrayAsync(uri);
        }

        public async Task<Stream> GetStream(Uri uri)
        {
            return await HttpClientBuilder.HttpClient.GetStreamAsync(uri);
        }

        public async Task<string> GetString(Uri uri)
        {
            return await HttpClientBuilder.HttpClient.GetStringAsync(uri);
        }

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

        private async Task<HttpResponseMessage> GetWebResponseInternal(Func<CancellationToken, Task<HttpResponseMessage>> getResponse, CancellationToken token = default)
        {
            if (token == null)
                token = DefaultCancellationTokenSource().Token;
            HttpResponseMessage response = await getResponse(token);
            if (!response.IsSuccessStatusCode)
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
            if (!response.IsSuccessStatusCode) response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<IResponse<T>> CreateResponseInternal<T>(HttpResponseMessage response)
        {
            IResponseFactory factory = new ResponseFactory();
            return await factory.Create<T>(response);
        }

        public async Task<IResponse> Send(IRequest request, CancellationToken token = default)
        {
            if (token == null)
                token = DefaultCancellationTokenSource().Token;
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                using (HttpResponseMessage responseMessage = await HttpClientBuilder.HttpClient.SendAsync(requestMessage, token))
                {
                    responseMessage.EnsureSuccessStatusCode();
                    IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                    return await responseFactory.Create(responseMessage);
                }
            }                
        }

        public async Task<IResponse<T>> Send<T>(IRequest request, CancellationToken token = default)
        {
            if (token == null)
                token = DefaultCancellationTokenSource().Token;
            using (HttpRequestMessage requestMessage = await request.MessageBuilder.Build())
            {
                HttpResponseMessage responseMessage = await HttpClientBuilder.HttpClient.SendAsync(requestMessage, token);
                if (!responseMessage.IsSuccessStatusCode) responseMessage.EnsureSuccessStatusCode();
                IResponseFactory responseFactory = request.MessageBuilder.CreateResponseFactory();
                return await responseFactory.Create<T>(responseMessage);
            }
        }        

        private CancellationTokenSource DefaultCancellationTokenSource()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(20));
            return source;
        }
    }
}
