using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public interface IResponseFactory
    {
        IResponse<T> Create<T>(HttpResponseMessage responseMessage, T value, string text = null, object json = null);

        Task<IResponse> Create(HttpResponseMessage responseMessage);

        Task<IResponse<T>> Create<T>(HttpResponseMessage responseMessage);
    }
}
