using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi;

[ApiController]
[Route("api/server")]
public class ServerController : ControllerBase
{
    private readonly CurrencyDBContext _context;

    public ServerController(CurrencyDBContext context)
    {
        _context = context;
    }
    [HttpGet("TestException")]
    public IActionResult TestException()
    {
        if (String.IsNullOrEmpty(null))
        {
            throw new NotImplementedException("Exception Test....");
        }
        return Ok();
    }
}
