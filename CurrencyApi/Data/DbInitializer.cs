using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await SeedData(scope.ServiceProvider.GetService<CurrencyDBContext>(), scope.ServiceProvider.GetService<CoindeskApiHttpClient>());
    }

    public static async Task SeedData(CurrencyDBContext context, CoindeskApiHttpClient httpClient)
    {
        await context.Database.MigrateAsync();
        if (context.Currency.Any())
        {
            Console.WriteLine("Currency data already exist");
        }
        else
        {
            var item = await httpClient.GetTransformedCoindeskData();
            if (item.Any())
            {
                context.AddRange(item);
                context.SaveChanges();
            }
        }

        if (context.Translation.Any())
        {
            Console.WriteLine("Translation data already exist");
        }
        else
        {
            if (!context.Currency.Any())
            {
                Console.WriteLine("Currency is empty");
                return;
            }

            var data = new List<Translation>()
            {
                new Translation{
                    Table = "tbl_Currency",
                    RelationId = context.Currency.FirstOrDefault(c => c.Code == "USD").CurrencyId,
                    Language = "zh-TW",
                    Text = "美金"
                },  new Translation{
                    Table = "tbl_Currency",
                    RelationId = context.Currency.FirstOrDefault(c => c.Code == "GBP").CurrencyId,
                    Language = "zh-TW",
                    Text = "英鎊"
                },  new Translation{
                    Table = "tbl_Currency",
                    RelationId = context.Currency.FirstOrDefault(c => c.Code == "EUR").CurrencyId,
                    Language = "zh-TW",
                    Text = "歐元"
                },
            };
            context.Translation.AddRange(data);
            context.SaveChanges();
        }
    }
}