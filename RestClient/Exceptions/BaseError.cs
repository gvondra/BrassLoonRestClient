using System;
using System.Net;

namespace BrassLoon.RestClient.Exceptions
{
    /// <summary>
    /// Parent of RequestError and SererError
    /// </summary>
#pragma warning disable S3376 // Attribute, EventArgs, and Exception type names should end with the type being extended
    public abstract class BaseError : ApplicationException
    {
        private readonly IResponse _response;

        protected BaseError(IResponse response, string message)
            : base(message)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode => _response.StatusCode;
        public string Text => _response.Text;
        public IResponse Response => _response;
    }
#pragma warning restore S3376 // Attribute, EventArgs, and Exception type names should end with the type being extended
}
