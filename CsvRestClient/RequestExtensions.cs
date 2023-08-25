using BrassLoon.CsvRestClient.Internal;
using BrassLoon.RestClient;

namespace BrassLoon.CsvRestClient
{
    public static class RequestExtensions
    {
        public static IRequest AddCsvBody(this IRequest request, object body) => request.AddBody(new CsvRequestBody(body));
    }
}
