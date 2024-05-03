using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestApi
{
    public class CsvTextInputFormatter : TextInputFormatter
    {
        public CsvTextInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            bool returnCollection = false;
            Type modelType = context.ModelType;
            if (modelType.IsArray)
            {
                modelType = modelType.GetElementType();
                returnCollection = true;
            }
            else if (modelType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(modelType))
            {
                modelType = modelType.GetGenericArguments()[0];
                returnCollection = true;
            }
            IList records = (IList)Activator.CreateInstance(Type.GetType($"System.Collections.Generic.List`1[[{modelType.AssemblyQualifiedName}]]"));
            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using StreamReader streamReader = new StreamReader(context.HttpContext.Request.Body, encoding);
            using CsvReader reader = new CsvReader(streamReader, csvConfiguration, false);
            await foreach (object record in reader.GetRecordsAsync(modelType))
            {
                records.Add(record);
            }
            if (context.ModelType.IsArray)
            {
                Array recordArray = Array.CreateInstance(modelType, records.Count);
                records.CopyTo(recordArray, 0);
                records = recordArray;
            }
            if (returnCollection)
                return InputFormatterResult.Success(records);
            else if (records.Count > 0)
                return InputFormatterResult.Success(records[0]);
            else
                return InputFormatterResult.NoValue();
        }
    }
}
