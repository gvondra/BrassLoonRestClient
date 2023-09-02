using BrassLoon.RestClient;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrassLoon.CsvRestClient.Internal
{
    public sealed class ResponseFactory : IResponseFactory
    {
        private readonly IResponseFactory _innerResponseFactory;

        internal ResponseFactory(IResponseFactory responseFactory)
        {
            _innerResponseFactory = responseFactory;
        }

        public Task<IResponse> Create(HttpResponseMessage responseMessage)
            => _innerResponseFactory.Create(responseMessage);

        public async Task<IResponse<T>> Create<T>(HttpResponseMessage responseMessage)
        {
            T value = default(T);
            string text = null;
            IResponse<T> response = null;
            if (responseMessage.Content.Headers.ContentLength.HasValue
                && responseMessage.Content.Headers.ContentLength.Value > 0L
                && responseMessage.Content.Headers.ContentType != null)
            {
                switch (responseMessage.Content.Headers.ContentType.MediaType.ToLower(CultureInfo.InvariantCulture))
                {
                    case "text/csv":
                        (T Value, string Text) valueText = await Deserialize<T>(responseMessage);
                        value = valueText.Value;
                        text = valueText.Text;
                        break;
                    default:
                        response = await _innerResponseFactory.Create<T>(responseMessage);
                        break;
                }
            }
            if (response == null)
                response = Create<T>(responseMessage, value, text);
            return response;
        }

        public IResponse<T> Create<T>(HttpResponseMessage responseMessage, T value, string text = null, object json = null) => _innerResponseFactory.Create(responseMessage, value, text, json);

        private static async Task<(T, string)> Deserialize<T>(HttpResponseMessage responseMessage)
        {
            bool isArray = false;
            bool returnCollection = false;
            Type modelType = typeof(T);
            if (modelType.IsArray)
            {
                modelType = modelType.GetElementType();
                returnCollection = true;
                isArray = true;
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
            string text = await responseMessage.Content.ReadAsStringAsync();
            using (StringReader stringReader = new StringReader(text))
            {
                using (CsvReader reader = new CsvReader(stringReader, csvConfiguration, false))
                {
                    foreach (object record in reader.GetRecords(modelType))
                    {
                        records.Add(record);
                    }
                }
            }
            if (isArray)
            {
                Array recordArray = Array.CreateInstance(modelType, records.Count);
                records.CopyTo(recordArray, 0);
                records = recordArray;
            }
            if (returnCollection)
                return ((T)records, text);
            else if (records.Count > 0)
                return ((T)records[0], text);
            else
                return (default(T), text);
        }
    }
}
