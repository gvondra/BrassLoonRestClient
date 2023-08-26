using BrassLoon.RestClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.CsvRestClient.Internal
{
    public sealed class RequestMessageBuilder : IRequestMessageBuilder
    {
        private readonly IRequestMessageBuilder _innerMessageBuilder;

        internal RequestMessageBuilder(IRequestMessageBuilder messageBuilder)
        {
            _innerMessageBuilder = messageBuilder;
        }

        public Task<HttpRequestMessage> Build() => _innerMessageBuilder.Build();

        public IResponseFactory CreateResponseFactory() => new ResponseFactory(_innerMessageBuilder.CreateResponseFactory());
    }
}
