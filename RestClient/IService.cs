using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public interface IService
    {
        Task<IResponse> Delete(Uri uri, CancellationToken token = default);
        Task<IResponse<T>> Delete<T>(Uri uri, CancellationToken token = default);

        Task<IResponse> Get(Uri uri, CancellationToken token = default);
        Task<IResponse<T>> Get<T>(Uri uri, CancellationToken token = default);
        Task<byte[]> GetBytes(Uri uri);
        Task<Stream> GetStream(Uri uri);
        Task<string> GetString(Uri uri);

        Task<IResponse<T>> Post<T>(Uri uri, object body = null, CancellationToken token = default);
        Task<IResponse> Post(Uri uri, object body = null, CancellationToken token = default);
        Task<IResponse> Put(Uri uri, object body = null, CancellationToken token = default);
        Task<IResponse<T>> Put<T>(Uri uri, object body = null, CancellationToken token = default);
        Task<IResponse> Send(IRequest request, CancellationToken token = default);
        Task<IResponse<T>> Send<T>(IRequest request, CancellationToken token = default);

        void CancelPendingRequests();
        IRequest CreateRequest(Uri address, HttpMethod method);
        IRequest CreateRequest(Uri address, HttpMethod method, object body);
    }
}
