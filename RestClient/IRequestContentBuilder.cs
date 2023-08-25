using System.Net.Http;

namespace BrassLoon.RestClient
{
    public interface IRequestContentBuilder
    {
        HttpContent Build();
    }
}
