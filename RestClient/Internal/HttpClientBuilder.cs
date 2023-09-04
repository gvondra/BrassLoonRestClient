using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BrassLoon.RestClient.Internal
{
    internal sealed class HttpClientBuilder
    {
        private static readonly TimeSpan _defaultTimout = TimeSpan.FromMinutes(5);
        private static readonly Dictionary<double, HttpClient> _httpClients = new Dictionary<double, HttpClient>();

        public static HttpClient Get(TimeSpan? timeout = null)
        {
            if (!timeout.HasValue || timeout.Value.Equals(TimeSpan.Zero))
                timeout = _defaultTimout;
            if (!_httpClients.ContainsKey(timeout.Value.TotalMilliseconds))
            {
                lock (_httpClients)
                {
                    if (!_httpClients.ContainsKey(timeout.Value.TotalMilliseconds))
                        _httpClients[timeout.Value.TotalMilliseconds] = Create(timeout.Value);
                }
            }
            return _httpClients[timeout.Value.TotalMilliseconds];
        }

        public static IEnumerable<HttpClient> GetEnumerator() => _httpClients.Select(i => i.Value);

        public static HttpClient Create(TimeSpan timeout)
            => Create(timeout, DefaultClientHandler());

        public static HttpClient Create(TimeSpan timeout, HttpClientHandler handler)
            => new HttpClient(handler) { Timeout = timeout };

        public static HttpClientHandler DefaultClientHandler()
        {
            return new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                PreAuthenticate = true,
                UseCookies = false,
                UseDefaultCredentials = false
            };
        }

        internal static void ClearCache()
        {
            lock (_httpClients)
            {
                foreach (KeyValuePair<double, HttpClient> keyValuePair in _httpClients)
                {
                    _httpClients.Remove(keyValuePair.Key);
                    keyValuePair.Value.Dispose();
                }
            }
        }
    }
}
