using BrassLoon.CsvRestClient.Internal;
using BrassLoon.RestClient;

namespace BrassLoon.CsvRestClient
{
    public static class RequestExtensions
    {
        public static IRequest AddCsvBody(this IRequest request, object body) => request.AddBody(new CsvRequestBody(body));
        public static IRequest AcceptCSV(this IRequest request)
        {
#pragma warning disable IDE0078 // Use pattern matching
#pragma warning disable IDE0083 // Use pattern matching
            if (!(request is Request))
            {
                request = new Request(request);
            }
            request.Accept = "text/csv";
            return request;
#pragma warning restore IDE0083 // Use pattern matching
#pragma warning restore IDE0078 // Use pattern matching
        }
    }
}
