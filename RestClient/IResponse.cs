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
        /// <summary>
        /// For text/plain and application/problem+json responses, contains the message body text
        /// </summary>
        string Text { get; }
        /// <summary>
        /// For application/json responses, contains the response message body text
        /// </summary>
        object Json { get; }
    }

    public interface IResponse<T> : IResponse
    {
        T Value { get; }
    }
}
