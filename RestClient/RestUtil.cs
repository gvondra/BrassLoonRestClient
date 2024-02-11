using BrassLoon.RestClient.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BrassLoon.RestClient
{
    public class RestUtil
    {
        public virtual async Task<T> Send<T>(IService service, IRequest request, CancellationToken token = default)
        {
            IResponse<T> response = await service.Send<T>(request, token);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Post<T>(IService service, Uri uri, object body, CancellationToken token = default)
        {
            IResponse<T> response = await service.Post<T>(uri, body, token);
            CheckSuccess(response);
            return response.Value;
        }

        public virtual async Task<T> Get<T>(IService service, Uri uri, CancellationToken token = default)
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
                char indicator = ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture)[0];
                if (indicator == '4')
                    exception = new RequestError(response);
                else
                    exception = new ServerError(response);
                AddRequestAddress(exception.Data, response.Message);
                AddText(exception.Data, response);
                throw exception;
            }
        }

        private static void AddRequestAddress(IDictionary data, HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage?.RequestMessage?.RequestUri != null)
                data["RequestAddress"] = httpResponseMessage.RequestMessage.RequestUri.ToString();
        }

        private static void AddText(IDictionary data, IResponse response)
        {
            if (response?.Text != null)
                data["Text"] = response.Text;
        }

        public virtual string AppendPath(string basePath, params string[] segments)
        {
            UriBuilder builder = new UriBuilder(basePath);
            List<string> path = builder.Path.Split('/')
                .Concat(segments)
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();
            path = path.Take(path.Count - 1)
                .Select(p => p.Trim('/'))
                .Concat(
                path.Skip(path.Count - 1)
                .Select(p => p.TrimStart('/')))
                .ToList();
            builder.Path = string.Join("/", path);
            return builder.ToString();
        }
    }
}
