using AutoMapper;
using Newtonsoft.Json;

namespace CurrencyApi;

public class CoindeskApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly CurrencyDBContext _context;

    public CoindeskApiHttpClient(HttpClient httpClient, IConfiguration config, CurrencyDBContext context)
    {
        _httpClient = httpClient;
        _config = config;
        _context = context;
    }

    public async Task<List<Currency>> GetTransformedCoindeskData()
    {
        var audit = new AuditTrail();

        audit.ReqeustDateUTC = DateTime.UtcNow;
        audit.ClientIP = _httpClient.BaseAddress?.ToString();
        audit.Controller = _config["CoindeskAPI"];

        var response = await _httpClient.GetFromJsonAsync<CoindeskResponseDto>(audit.Controller);
        audit.ResponseDateUTC = DateTime.UtcNow;
        audit.Response = JsonConvert.SerializeObject(response);
        audit.HttpStatus = 200;
        _context.AuditTrail.Add(audit);
        await _context.SaveChangesAsync();
        return response.bpi.Select(x => new Currency()
        {
            Code = x.Value.code,
            Name = x.Value.description,
            Rate = x.Value.rate_float,
            UpdatedUTC = response.time.updatedISO.ToUniversalTime(),
        }).ToList(); ;
    }
}
