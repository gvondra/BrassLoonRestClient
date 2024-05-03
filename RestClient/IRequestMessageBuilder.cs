using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public interface IRequestMessageBuilder
    {
        Task<HttpRequestMessage> Build();

        IResponseFactory CreateResponseFactory();
    }
}
