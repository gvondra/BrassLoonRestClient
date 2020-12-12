using BrassLoon.RestClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public static class Program
    {
        public static async Task Main()
        {
            try
            {
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

        public class Result
        {
            public string Name { get; set; }
        }
    }
}
