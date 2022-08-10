using Microsoft.AspNetCore.Mvc;
using PollyExampleApi.Services;

namespace PollyExampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpClientPollyController : ControllerBase
    {
        private readonly IService _service;

        public HttpClientPollyController(IService service)
        {
            _service = service;
        }

        [HttpGet("{code:int}")]
        public async Task<IActionResult> Get([FromRoute] int code)
        {
            await _service.Get(code);
            return Ok();
        }
    }
}
