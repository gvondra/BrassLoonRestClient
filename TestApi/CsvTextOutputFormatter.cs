using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace TestApi
{
    public class CsvTextOutputFormatter : TextOutputFormatter
    {
        public CsvTextOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            bool writeHeader = true;
            object value = context.Object;
            if (value is not IEnumerable)
                value = new List<object> { value };
            StringBuilder buffer = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(buffer);
            using CsvWriter writer = new CsvWriter(stringWriter, CultureInfo.InvariantCulture, false);
            TypeConverterOptions typeConverterOptions = new TypeConverterOptions { Formats = new string[] { "O" } };
            writer.Context.TypeConverterOptionsCache.AddOptions<DateTime>(typeConverterOptions);
            writer.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(typeConverterOptions);
            IEnumerator enumerator = ((IEnumerable)value).GetEnumerator();
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
            await writer.FlushAsync();
            await context.HttpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }
    }
}
