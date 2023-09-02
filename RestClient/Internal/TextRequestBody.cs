namespace BrassLoon.RestClient.Internal
{
    public sealed class TextRequestBody : IRequestBody
    {
        private readonly string _body;

        internal TextRequestBody(string body)
        {
            _body = body;
        }

        public string Body => _body;

        public IRequestContentBuilder CreateContentBuilder() => new TextRequestContentBuilder(this);
    }
}
