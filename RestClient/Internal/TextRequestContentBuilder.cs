using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace BrassLoon.RestClient.Internal
{
    public class TextRequestContentBuilder : IRequestContentBuilder
    {
        private readonly TextRequestBody _body;

        internal TextRequestContentBuilder(TextRequestBody body)
        {
            _body = body;
        }

        public HttpContent Build()
        {
            return Build(_body?.Body);
        }

        public static HttpContent Build(string body)
        {
            MemoryStream stream = new MemoryStream();
            if (body != null)
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.Write(body);
                writer.Flush();
                stream.Position = 0;
            }
            HttpContent result = new StreamContent(stream);
            result.Headers.Add("Content-Type", "text/plain");
            return result;
        }

        public static string Deserialize(Stream stream)
        {
            string value = string.Empty;
            if (stream != null && stream.Length > 0)
            {
                using (TextReader textReader = new StreamReader(stream, Encoding.UTF8))
                {
                    value = textReader.ReadToEnd();
                }
            }
            return value;
        }
    }
}
