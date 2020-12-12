using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BrassLoon.RestClient
{
    public interface IResponse : IDisposable
    {
        HttpResponseMessage Message { get; }
    }

    public interface IResponse<T> : IResponse
    {
        T Value { get; }
    }
}
