using System;
using System.Net.Http;

namespace BrassLoon.RestClient.Internal
{
    internal sealed class HttpClientBuilder
    {
        private static readonly HttpClient _httpClient;

        static HttpClientBuilder()
        {
            _httpClient = new HttpClient(
                new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                    PreAuthenticate = true,
                    UseCookies = false,
                    UseDefaultCredentials = true
                })
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        public static HttpClient HttpClient => _httpClient;
    }
}
