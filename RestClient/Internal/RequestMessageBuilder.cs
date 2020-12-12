using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Internal
{
    public sealed class RequestMessageBuilder : IRequestMessageBuilder
    {
        private readonly UrlProcessor _urlProcessor = new UrlProcessor();
        private readonly Request _request;

        internal RequestMessageBuilder(Request request)
        {
            _request = request;
        }

        public async Task<HttpRequestMessage> Build()
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(_request.Method, _urlProcessor.CreateUri(_request));
            AddHeaders(requestMessage);
            if (_request.RequestBody != null)
                requestMessage.Content = _request.RequestBody.CreateContentBuilder().Build();
            if (_request.GetJwtAuthorizationToken != null)
                requestMessage.Headers.Add("Authorization", string.Format("Bearer {0}", await _request.GetJwtAuthorizationToken()));
            return requestMessage;
        }

        public IResponseFactory CreateResponseFactory() => new ResponseFactory();

        private void AddHeaders(HttpRequestMessage requestMessage)
        {
            foreach ((string, string) header in _request.Headers)
            {
                requestMessage.Headers.Add(header.Item1, header.Item2);
            }
        }
    }
}
