using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private static dynamic Result = new { Name = "Maple" };

        [HttpDelete()]
        public IActionResult Delete()
        {
            return Ok(Result);
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(Result);
        }

        [HttpGet("Name")]
        public IActionResult GetName()
        {
            return Content(Result.Name, "text/plain");
        }

        [HttpGet("Branch/{id}")]
        public IActionResult GetBranch(string id)
        {
            return Ok(new { Branch = 404, Id = id });
        }

        [HttpPost(), HttpPut()]
        public IActionResult Create([FromBody] dynamic body)
        {
            return Ok(Result);
        }
    }
}
