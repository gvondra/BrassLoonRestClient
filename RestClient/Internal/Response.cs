﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BrassLoon.RestClient.Internal
{
    public class Response : IResponse
    {
        private bool _disposedValue;
        private HttpResponseMessage _message;

        internal Response(HttpResponseMessage message)
        {
            _message = message;
        }

        public HttpResponseMessage Message => _message;

        public HttpStatusCode StatusCode => _message?.StatusCode ?? default(HttpStatusCode);

        public bool IsSuccessStatusCode => _message?.IsSuccessStatusCode ?? false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_message != null)
                    {
                        _message.Dispose();
                        _message = null;
                    }
                }

                // set large fields to null
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public sealed class Response<T> : Response, IResponse<T>
    {
        private readonly T _value;

        internal Response(HttpResponseMessage message, T value) : base(message)
        {
            _value = value;
        }

        public T Value => _value;
    }
}
