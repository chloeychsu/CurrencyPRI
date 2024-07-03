using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;

public class CurrencyDBContext : DbContext
{
    public CurrencyDBContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Translation> Translation { get; set; }
}
