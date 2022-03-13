using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace BrassLoon.RestClient
{
    public interface IResponse : IDisposable
    {
        HttpStatusCode StatusCode { get; }
        HttpResponseMessage Message { get; }
        bool IsSuccessStatusCode { get; }
    }

    public interface IResponse<T> : IResponse
    {
        T Value { get; }
        string Text { get; }
    }
}
