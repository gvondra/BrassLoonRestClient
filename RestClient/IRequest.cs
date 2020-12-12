using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public interface IRequest
    {
        IRequestMessageBuilder MessageBuilder { get; }

        IRequest AddJsonBody(object body);
        IRequest AddHeader(string name, string value);
        IRequest AddPath(string path);
        IRequest AddPathParameter(string name, string value);
        IRequest AddQueryParameter(string name, string value);
        IRequest AddJwtAuthorizationToken(Func<string> getToken);
        IRequest AddJwtAuthorizationToken(Func<Task<string>> getToken);        
    }
}
