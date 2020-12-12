using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public interface IResponseFactory
    {
        Task<IResponse> Create(HttpResponseMessage responseMessage);
        Task<IResponse<T>> Create<T>(HttpResponseMessage responseMessage);
    }
}
