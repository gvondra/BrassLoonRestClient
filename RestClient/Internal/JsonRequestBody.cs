namespace BrassLoon.RestClient.Internal
{
    public sealed class JsonRequestBody : IRequestBody
    {
        private readonly object _body;

        public JsonRequestBody(object body)
        {
            _body = body;
        }

        public object Body => _body;

        public IRequestContentBuilder CreateContentBuilder() => new JsonRequestContentBuilder(this);
    }
}
