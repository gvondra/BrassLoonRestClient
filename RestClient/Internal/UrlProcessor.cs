using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BrassLoon.RestClient.Internal
{
    public static class UrlProcessor
    {
        public static Uri CreateUri(Request request)
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

        private static Uri AppendPaths(Uri addreess, List<string> paths)
        {
            UriBuilder builder = new UriBuilder(addreess);
            builder.Path = string.Join("/",
                addreess.Segments.Where(seg => seg != "/")
                .Concat(paths)
                );
            return builder.Uri;
        }

        private static Uri ReplacePathVariables(Uri addreess, Dictionary<string, string> parameters)
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

        public static Uri AppendQueryParameters(Uri addreess, Dictionary<string, string> parameters)
        {
            UriBuilder builder = new UriBuilder(addreess);
            List<string> pairs = new List<string>();
            if (!string.IsNullOrEmpty(builder.Query))
                pairs.Add(builder.Query.TrimStart('?'));
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                pairs.Add($"{WebUtility.UrlEncode(keyValue.Key)}={WebUtility.UrlEncode(keyValue.Value ?? string.Empty)}");
            }
            if (pairs.Count > 0)
                /*
                 * The UriBuilder's set query method behaves differently between .net core and .net framework
                 * Both are trying to add a question mark to the begining. But they differ when there's 
                 * already a question mark at the start of the string.
                 * In .net framework, if the query string passed in starts with question mark then another 
                 * question mark is pre-pended leaving you with 2 question marks. (these caused me issues)
                 * In .net core, if the query string passed in starts with a question mark then no addition 
                 * question mark is added.
                 */
                builder.Query = string.Join("&", pairs.ToArray());
            return builder.Uri;
        }
    }
}
