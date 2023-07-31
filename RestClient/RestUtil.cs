using BrassLoon.RestClient.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public class RestUtil
    {
        public virtual async Task<T> Send<T>(IService service, IRequest request)
        {
            IResponse<T> response = await service.Send<T>(request);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Send<T>(IService service, IRequest request, CancellationToken? token = null)
        {
            IResponse<T> response = await service.Send<T>(request, token);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Post<T>(IService service, Uri uri, object body)
        {
            IResponse<T> response = await service.Post<T>(uri, body);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Post<T>(IService service, Uri uri, object body, CancellationToken? token = null)
        {
            IResponse<T> response = await service.Post<T>(uri, body, token);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Get<T>(IService service, Uri uri)
        {
            IResponse<T> response = await service.Get<T>(uri);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Get<T>(IService service, Uri uri, CancellationToken? token = null)
        {
            IResponse<T> response = await service.Get<T>(uri, token);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual void CheckSuccess(IResponse response)
        {
            Exception exception;
            if (!response.IsSuccessStatusCode)
            {
                char indicator = ((int)response.StatusCode).ToString()[0];
                if (indicator == '4')                    
                    exception = new RequestError(response);
                else
                    exception = new ServerError(response);
                AddRequestAddress(exception.Data, response.Message);
                AddText(exception.Data, response);
                throw exception;
            }
        }

        private IDictionary AddRequestAddress(IDictionary data, HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage?.RequestMessage?.RequestUri != null)
                data["RequestAddress"] = httpResponseMessage.RequestMessage.RequestUri.ToString();
            return data;
        }

        private IDictionary AddText(IDictionary data, IResponse response)
        {
            if (response?.Text != null)
                data["Text"] = response.Text;
            return data;
        }

        public virtual string AppendPath(string basePath, params string[] segments)
        {
            List<string> path = basePath.Split('/').Where(p => !string.IsNullOrEmpty(p)).ToList();
            return string.Join("/",
                path.Concat(segments.Where(s => !string.IsNullOrEmpty(s)))
                );
        }
    }
}
