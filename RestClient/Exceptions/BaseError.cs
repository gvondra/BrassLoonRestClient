using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Exceptions
{
    /// <summary>
    /// Parent of RequestError and SererError
    /// </summary>
    public abstract class BaseError : ApplicationException
    {
        private readonly IResponse _response;

        public BaseError(IResponse response, string message)
            : base(message)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode => _response.StatusCode;
        public string Text => _response.Text;
        public IResponse Response => _response;
    }
}
