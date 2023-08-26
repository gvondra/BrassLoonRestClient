using BrassLoon.RestClient;
using System;
using System.Threading.Tasks;

namespace BrassLoon.CsvRestClient.Internal
{
    public sealed class Request : IRequest
    {
        private readonly IRequest _innerRequest;
        private readonly IRequestMessageBuilder _messageBuilder;

        internal Request(IRequest request)
        {
            _innerRequest = request;
            _messageBuilder = new RequestMessageBuilder(request.MessageBuilder);
        }

        public string Accept { get => _innerRequest.Accept; set => _innerRequest.Accept = value; }

        public IRequestMessageBuilder MessageBuilder => _messageBuilder;

        public IRequest AddBody(IRequestBody body)
        {
            _innerRequest.AddBody(body);
            return this;
        }

        public IRequest AddHeader(string name, string value)
        {
            _innerRequest.AddHeader(name, value);
            return this;
        }

        public IRequest AddJsonBody(object body) => _innerRequest.AddJsonBody(body);

        public IRequest AddJwtAuthorizationToken(Func<string> getToken)
        {
            _innerRequest.AddJwtAuthorizationToken(getToken);
            return this;
        }

        public IRequest AddJwtAuthorizationToken(Func<Task<string>> getToken)
        {
            _innerRequest.AddJwtAuthorizationToken(getToken);
            return this;
        }

        public IRequest AddPath(string path)
        {
            _innerRequest.AddPath(path);
            return this;
        }

        public IRequest AddPathParameter(string name, string value)
        {
            _innerRequest.AddPathParameter(name, value);
            return this;
        }

        public IRequest AddQueryParameter(string name, string value)
        {
            _innerRequest.AddQueryParameter(name, value);
            return this;
        }
    }
}
