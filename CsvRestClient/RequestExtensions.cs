using BrassLoon.CsvRestClient.Internal;
using BrassLoon.RestClient;

namespace BrassLoon.CsvRestClient
{
    public static class RequestExtensions
    {
        public static IRequest AddCsvBody(this IRequest request, object body) => request.AddBody(new CsvRequestBody(body));
        public static IRequest AcceptCSV(this IRequest request)
        {
            if (!(request is Request))
            {
                request = new Request(request);
            }
            request.Accept = "text/csv";
            return request;
        }
    }
}
