using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{

		[HttpGet("{text}")]
		public async Task<ActionResult> Get(string text)
		{
			return Ok($"Received value : {text}");
		}
	}
}
