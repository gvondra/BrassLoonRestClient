using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BrassLoon.RestClient.Internal
{
    internal sealed class HttpClientBuilder
    {
        private readonly static HttpClient _httpClient;

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
