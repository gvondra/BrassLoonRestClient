using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Internal
{
    public sealed class Request : IRequest
    {
        private readonly IRequestMessageBuilder _requestMessageBuilder;
        private readonly Uri _baseAddress;
        private readonly TimeSpan? _timeout;
        private readonly HttpMethod _method;
        private readonly List<(string, string)> _headers = new List<(string, string)>();
        private readonly List<string> _paths = new List<string>();
        private readonly Dictionary<string, string> _pathParameters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
        private Func<Task<string>> _getJwtAuthorizationToken;
        private IRequestBody _requestBody;

        internal Request(
            Uri baseAddress,
            TimeSpan? timeout,
            HttpMethod method)
        {
            _requestMessageBuilder = new RequestMessageBuilder(this);
            _baseAddress = baseAddress;
            _timeout = timeout;
            _method = method;
        }

        internal Request(
            Uri baseAddress,
            HttpMethod method)
            : this(baseAddress, null, method)
        { }

        public IRequestMessageBuilder MessageBuilder => _requestMessageBuilder;
        public string Accept { get; set; } = "application/json";
        public Uri BaseAddress => _baseAddress;
        public TimeSpan? Timeout => _timeout;
        internal HttpMethod Method => _method;
        internal List<(string, string)> Headers => _headers;
        internal List<string> Paths => _paths;
        internal Dictionary<string, string> PathParameters => _pathParameters;
        internal Dictionary<string, string> QueryParameters => _queryParameters;
        internal Func<Task<string>> GetJwtAuthorizationToken => _getJwtAuthorizationToken;
        internal IRequestBody RequestBody => _requestBody;

        public IRequest AddHeader(string name, string value)
        {
            _headers.Add((name, value));
            return this;
        }

        public IRequest AddBody(IRequestBody body)
        {
            _requestBody = body;
            return this;
        }

        public IRequest AddJsonBody(object body)
            => AddBody(new JsonRequestBody(body));

        public IRequest AddJwtAuthorizationToken(Func<string> getToken)
        {
            _getJwtAuthorizationToken = () => Task.FromResult(getToken());
            return this;
        }

        public IRequest AddJwtAuthorizationToken(Func<Task<string>> getToken)
        {
            _getJwtAuthorizationToken = getToken;
            return this;
        }

        public IRequest AddPath(string path)
        {
            _paths.Add(path ?? string.Empty);
            return this;
        }

        public IRequest AddPathParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            _pathParameters[name] = value ?? string.Empty;
            return this;
        }

        public IRequest AddQueryParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            _queryParameters[name] = value ?? string.Empty;
            return this;
        }
    }
}
