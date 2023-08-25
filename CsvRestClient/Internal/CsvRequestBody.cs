using BrassLoon.RestClient;

namespace BrassLoon.CsvRestClient.Internal
{
    public sealed class CsvRequestBody : IRequestBody
    {
        private readonly object _body;

        public CsvRequestBody(object body)
        {
            _body = body;
        }

        public object Body => _body;

        public IRequestContentBuilder CreateContentBuilder()
        {
            return new CsvRequestContentBuilder(this);
        }
    }
}
