using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;

[ApiController]
[Route("api/server")]
public class ServerController : ControllerBase
{
    private readonly CurrencyDBContext _context;
    private readonly CoindeskApiHttpClient _coindeskHttpClient;

    public ServerController(CurrencyDBContext context, CoindeskApiHttpClient coindeskHttpClient)
    {
        _context = context;
        _coindeskHttpClient = coindeskHttpClient;
    }

    [HttpGet("GetCoindeskData")]
    public async Task<ActionResult<List<Currency>>> GetCoindeskData()
    {
        return await _coindeskHttpClient.GetTransformedCoindeskData();
    }
    [HttpGet("GetAllActionTrail")]
    public async Task<ActionResult<List<AuditTrail>>> GetAllActionTrail()
    {
        var query = await _context.AuditTrail.OrderBy(x => x.ReqeustDateUTC).ToListAsync();
        return query;
    }
    [HttpGet("TestException")]
    public IActionResult TestException()
    {
        if (String.IsNullOrEmpty(null))
        {
            throw new NotImplementedException("Exception Test.....");
        }
        return Ok();
    }
}
