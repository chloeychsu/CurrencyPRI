using AutoMapper;

namespace CurrencyApi;

public class CoindeskApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public CoindeskApiHttpClient(HttpClient httpClient,IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Currency>> GetTransformedCoindeskData()
    {
        var response = await _httpClient.GetFromJsonAsync<CoindeskResponseDto>(_config["CoindeskAPI"]);
        var item =response.bpi.Select(x => new Currency(){
            Code = x.Value.code,
            Name = x.Value.description,
            Rate = x.Value.rate_float,
            UpdatedISO = response.time.updatedISO
        }).ToList();
        return item;
    }
}
