using System.Globalization;
using CurrencyApi;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddControllers(opt => opt.Filters.Add<ActionResultFilter>())
.AddDataAnnotationsLocalization(opt=>{
    opt.DataAnnotationLocalizerProvider=(type,factory)=>factory.Create(typeof(CurrencyController));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB Connection
builder.Services.AddDbContext<CurrencyDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});
// Localization
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("zh-TW") };
    opt.DefaultRequestCulture = new RequestCulture("en-US");
    opt.SupportedCultures = supportedCultures;
    opt.SupportedUICultures = supportedCultures;
});
// DI
builder.Services.AddHttpClient<CoindeskApiHttpClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRequestLocalization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

await app.RunAsync();
