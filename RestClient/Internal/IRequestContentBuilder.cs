using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BrassLoon.RestClient.Internal
{
    public interface IRequestContentBuilder
    {
        HttpContent Build();
    }
}
