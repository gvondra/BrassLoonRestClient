using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private static readonly dynamic Result = new { Name = "Maple" };

        [HttpDelete()]
        public IActionResult Delete() => Ok(Result);

        [HttpGet()]
        public IActionResult Get() => Ok(Result);

        [HttpGet("Empty")]
        public IActionResult GetEmpty() => Ok();

        [HttpGet("Null")]
        public IActionResult GetNull() => Ok(null);

        [HttpGet("NoContent")]
        public IActionResult GetNoContent() => NoContent();

        [HttpGet("Name")]
        public IActionResult GetName() => Content(Result.Name, "text/plain");

        [HttpGet("Branch/{id}")]
        public IActionResult GetBranch(string id) => Ok(new { Branch = 404, Id = id });

#pragma warning disable IDE0060 // Remove unused parameter
        [HttpPost(), HttpPut()]
        public IActionResult Create([FromBody] dynamic body) => Ok(Result);
#pragma warning restore IDE0060 // Remove unused parameter

        [HttpPost("Leaves")]
        public IActionResult Post([FromBody] Leaves[] body) => Ok(body);
    }
}
