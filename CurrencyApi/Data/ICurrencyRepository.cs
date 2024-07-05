namespace CurrencyApi;

public interface ICurrencyRepository
{
    Task<List<CurrencyDto>> GetCurrenciesAsync();
    Task<CurrencyDto> GetCurrencyByCodeAsync(string code);
    Task<Currency> GetCurrencyEntityByCode(string code);
    void AddCurrency(Currency currency);
    void RemoveCurrency(Currency currency);
    Task<bool> SaveChangesAsync();
}
