using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net.Http;

namespace BrassLoon.RestClient.Internal
{
    public sealed class JsonRequestContentBuilder : IRequestContentBuilder
    {
        private readonly JsonRequestBody _body;

        internal JsonRequestContentBuilder(JsonRequestBody body)
        {
            _body = body;
        }

        public static HttpContent Build(object body)
        {
            MemoryStream stream = new MemoryStream();
            if (body != null)
            {
                StreamWriter writer = new StreamWriter(stream);
                JsonSerializer serializer = new JsonSerializer() { ContractResolver = new DefaultContractResolver() };
                serializer.Serialize(writer, body);
                writer.Flush();
                stream.Position = 0;
            }
            HttpContent result = new StreamContent(stream);
            result.Headers.Add("Content-Type", "application/json");
            return result;
        }

        public static T Deserialize<T>(Stream stream)
        {
            T value = default(T);
            if (stream != null && stream.Length > 0)
            {
                using (TextReader textReader = new StreamReader(stream))
                {
                    using (JsonReader reader = new JsonTextReader(textReader))
                    {
                        JsonSerializer serializer = new JsonSerializer() { ContractResolver = new DefaultContractResolver() };
                        value = serializer.Deserialize<T>(reader);
                    }
                }
            }
            return value;
        }

        public static object Deserialize(Stream stream)
        {
            object value = null;
            if (stream != null && stream.Length > 0)
            {
                using (TextReader textReader = new StreamReader(stream))
                {
                    using (JsonReader reader = new JsonTextReader(textReader))
                    {
                        JsonSerializer serializer = new JsonSerializer() { ContractResolver = new DefaultContractResolver() };
                        value = serializer.Deserialize(reader);
                    }
                }
            }
            return value;
        }

        public HttpContent Build() => Build(_body?.Body);
    }
}
