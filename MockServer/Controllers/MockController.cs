using Microsoft.AspNetCore.Mvc;

namespace MockServer.Controllers
{
    [ApiController]
    public class MockController : ControllerBase
    {
        [HttpPost("/api/v2/write")]
        public IActionResult Write()
        {
            return NoContent();
        }

        [HttpPost("/api/v2/query")]
        public IActionResult Query()
        {
            return File("response.csv", "text/csv");
        }
    }
}
