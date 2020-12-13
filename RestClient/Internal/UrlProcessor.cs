using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BrassLoon.RestClient.Internal
{
    public sealed class UrlProcessor
    {
        public Uri CreateUri(Request request)
        {
            Uri address = request.BaseAddress;
            if (request.Paths.Count > 0)
                address = AppendPaths(address, request.Paths);
            if (request.PathParameters.Count > 0)
                address = ReplacePathVariables(address, request.PathParameters);
            if (request.QueryParameters.Count > 0)
                address = AppendQueryParameters(address, request.QueryParameters);
            return address;
        }

        private Uri AppendPaths(Uri addreess, List<string> paths)
        {
            UriBuilder builder = new UriBuilder(addreess);
            builder.Path = string.Join("/",
                addreess.Segments.Where(seg => seg != "/")
                .Concat(paths)
                );
            return builder.Uri;
        }

        private Uri ReplacePathVariables(Uri addreess, Dictionary<string, string> parameters)
        {
            UriBuilder builder = new UriBuilder(addreess);
            string path = WebUtility.UrlDecode(builder.Path);
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                path = path.Replace(
                    $"{{{keyValue.Key}}}",
                    WebUtility.UrlEncode(keyValue.Value ?? string.Empty)
                    );
            }
            builder.Path = path;
            return builder.Uri;
        }

        private Uri AppendQueryParameters(Uri addreess, Dictionary<string, string> parameters)
        {
            UriBuilder builder = new UriBuilder(addreess);
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                string encoded = builder.Query.Length == 0 ? string.Empty : "&";
                encoded += $"{WebUtility.UrlEncode(keyValue.Key)}={WebUtility.UrlEncode(keyValue.Value ?? string.Empty)}";
                builder.Query += encoded;
            }
            return builder.Uri;
        }
    }
}
