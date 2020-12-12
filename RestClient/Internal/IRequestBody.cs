using System;
using System.Collections.Generic;
using System.Text;

namespace BrassLoon.RestClient.Internal
{
    public interface IRequestBody
    {
        IRequestContentBuilder CreateContentBuilder();
    }
}
