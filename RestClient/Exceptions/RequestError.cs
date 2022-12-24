using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient.Exceptions
{
    /// <summary>
    /// Thrown on 400 errors
    /// </summary>
    public class RequestError : BaseError
    {
        public RequestError(IResponse response) : base(response, $"Error {(int)response.StatusCode} {response.StatusCode}") { }
    }
}
