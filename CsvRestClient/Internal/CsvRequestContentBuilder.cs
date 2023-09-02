using BrassLoon.RestClient;
using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;

namespace BrassLoon.CsvRestClient.Internal
{
    public sealed class CsvRequestContentBuilder : IRequestContentBuilder
    {
        private readonly CsvRequestBody _body;

        public CsvRequestContentBuilder(CsvRequestBody body)
        {
            _body = body;
        }

        public HttpContent Build() => Build(_body?.Body);

        public static HttpContent Build(object body)
        {
            MemoryStream stream = new MemoryStream();
            if (body != null)
            {
                Serialize(stream, body);
                stream.Position = 0;
            }
            HttpContent result = new StreamContent(stream);
            result.Headers.Add("Content-Type", "text/csv");
            return result;
        }

        private static void Serialize(Stream stream, object body)
        {
#pragma warning disable IDE0078 // Use pattern matching
#pragma warning disable IDE0083 // Use pattern matching
            bool writeHeader = true;
            if (!(body is IEnumerable))
                body = new List<object> { body };
            using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8, 10240, true))
            {
                using (CsvWriter writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true))
                {
                    TypeConverterOptions typeConverterOptions = new TypeConverterOptions { Formats = new string[] { "O" } };
                    writer.Context.TypeConverterOptionsCache.AddOptions<DateTime>(typeConverterOptions);
                    writer.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(typeConverterOptions);
                    IEnumerator enumerator = ((IEnumerable)body).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (writeHeader)
                        {
                            writer.WriteHeader(enumerator.Current.GetType());
                            writer.NextRecord();
                        }
                        writer.WriteRecord(enumerator.Current);
                        writer.NextRecord();
                        writeHeader = false;
                    }
                    writer.Flush();
                }
            }
#pragma warning restore IDE0083 // Use pattern matching
#pragma warning restore IDE0078 // Use pattern matching
        }
    }
}
