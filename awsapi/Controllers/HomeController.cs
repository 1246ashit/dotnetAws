using Microsoft.AspNetCore.Mvc;

namespace awsapi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {

        public HomeController()
        {

        }
        [HttpPost("Test")]
        public async Task<ActionResult> Test()
        {
            return Ok("hi");
        }


    }
}