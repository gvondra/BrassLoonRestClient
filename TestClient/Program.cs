using BrassLoon.CsvRestClient;
using BrassLoon.RestClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    public static class Program
    {
#pragma warning disable IDE0051 // Remove unused private members
        public static async Task Main()
        {
            try
            {
                await GetNoContent();
                await PostArray();
                await Delete();
                await Get();
                await Create();
                await Update();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static async Task Delete()
        {
            IService service = new Service();
            await service.Delete(new Uri("http://localhost:5000/api/Tree"));
            using IResponse<Result> r = await service.Delete<Result>(new Uri("http://localhost:5000/api/Tree"));
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Delete)
                .AddPath("Tree");
            await service.Send(request);
            using IResponse<Result> rr = await service.Send<Result>(request);
        }

        private static async Task GetNoContent()
        {
            IService service = new Service();
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api/Tree/Empty"), HttpMethod.Get);
            await service.Send(request);
            request = service.CreateRequest(new Uri("http://localhost:5000/api/Tree/Null"), HttpMethod.Get);
            await service.Send(request);
            request = service.CreateRequest(new Uri("http://localhost:5000/api/Tree/NoContent"), HttpMethod.Get);
            await service.Send(request);
        }

        private static async Task Get()
        {
            IService service = new Service();
            await service.Get(new Uri("http://localhost:5000/api/Tree"));
            using IResponse<Result> r = await service.Get<Result>(new Uri("http://localhost:5000/api/Tree"));
            string s = await service.GetString(new Uri("http://localhost:5000/api/Tree/Name"));
            byte[] b = await service.GetBytes(new Uri("http://localhost:5000/api/Tree/Name"));
            using (Stream m = await service.GetStream(new Uri("http://localhost:5000/api/Tree/Name")))
            {
                using (StreamReader reader = new StreamReader(m))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Get)
               .AddPath("Tree");
            await service.Send(request);
            using IResponse<Result> rr = await service.Send<Result>(request);
            request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Get)
               .AddPath("Tree/Name");
            string name = (await service.Send<string>(request)).Value;
            request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Get)
               .AddPath("Tree/Branch/{id}")
               .AddPathParameter("id", "17")
               ;
            object br = (await service.Send<object>(request)).Value;
        }

        private static async Task Create()
        {
            IService service = new Service();
            await service.Post(new Uri("http://localhost:5000/api/Tree"), new { Name = "Oak" });
            using IResponse<Result> r = await service.Post<Result>(new Uri("http://localhost:5000/api/Tree"), new { Name = "Oak" });
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Post)
               .AddPath("Tree")
               .AddJsonBody(new { Name = "Oak" });
            await service.Send(request);
            using IResponse<Result> rr = await service.Send<Result>(request);
        }

        private static async Task Update()
        {
            IService service = new Service();
            await service.Put(new Uri("http://localhost:5000/api/Tree"), new { Name = "Oak" });
            using IResponse<Result> r = await service.Put<Result>(new Uri("http://localhost:5000/api/Tree"), new { Name = "Oak" });
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Put)
               .AddPath("Tree")
               .AddJsonBody(new { Name = "Oak" });
            await service.Send(request);
            using IResponse<Result> rr = await service.Send<Result>(request);
        }

        private static async Task PostArray()
        {
            IService service = new Service();
            IRequest request = service.CreateRequest(new Uri("http://localhost:5000/api"), HttpMethod.Post);
            Leaves body = new Leaves { Id = "1441" };
            request = request.AddPath("Tree/Leaves")
                .AddCsvBody(new List<dynamic> { body })
                .AcceptCSV();
            IResponse<List<Leaves>> r = await service.Send<List<Leaves>>(request);
        }

        public class Leaves
        {
            public string Id { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
        }
#pragma warning restore IDE0051 // Remove unused private members
    }
}
