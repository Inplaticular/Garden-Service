using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Inplanticular.Garden_Service.WebAPI.Controllers; 


[Route("api/[controller]")]
[ApiController]
public class RedisController : ControllerBase
{
	private readonly IConnectionMultiplexer _redis;
    
	public RedisController(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	[HttpGet("foo")]
	public async Task<IActionResult> Foo()
	{
		var db = _redis.GetDatabase();
		var foo = await db.StringGetAsync("foo");
		return Ok(foo.ToString());
	}
}