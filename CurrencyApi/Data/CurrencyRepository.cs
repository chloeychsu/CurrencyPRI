
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly CurrencyDBContext _context;
    private readonly IMapper _mapper;

    public CurrencyRepository(CurrencyDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public void AddCurrency(Currency currency)
    {
        _context.Currencies.Add(currency);
    }

    public async Task<List<CurrencyDto>> GetCurrenciesAsync()
    {
        var query = _context.Currencies.OrderBy(x => x.Code).AsQueryable();
        if (!query.Any()) return new();
        return await query.ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<CurrencyDto> GetCurrencyByCodeAsync(string code)
    {
        return await _context.Currencies
            .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<Currency> GetCurrencyEntityByCode(string code)
    {
        return await _context.Currencies.Include(x => x.Language).FirstOrDefaultAsync(x => x.Code == code);

    }

    public void RemoveCurrency(Currency currency)
    {
        _context.Currencies.Remove(currency);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
