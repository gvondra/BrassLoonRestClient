﻿namespace BrassLoon.RestClient.Exceptions
{
    /// <summary>
    /// Thrown on 500 errors
    /// </summary>
    public class ServerError : BaseError
    {
        public ServerError(IResponse response)
            : base(response, $"Server Error {(int)response.StatusCode} {response.StatusCode}") { }
    }
}
