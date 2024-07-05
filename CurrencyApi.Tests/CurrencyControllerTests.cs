using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace CurrencyApi.Tests;

public class CurrencyControllerTests
{
    private readonly Mock<ICurrencyRepository> _currencyRepo;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CurrencyController> _Localizer;
    private readonly CurrencyController _controller;
    private readonly Fixture _fixture;
    public CurrencyControllerTests()
    {
        _currencyRepo = new Mock<ICurrencyRepository>();
        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        _Localizer = new StringLocalizer<CurrencyController>(new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance));
        _mapper = new Mapper(new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider);
        _fixture = new Fixture();
        _controller = new CurrencyController(_currencyRepo.Object, _mapper, _Localizer);
    }
    [Fact]
    public async Task GetAllCurrencies_ReturnsThreeCurrencies()
    {
        // Arrange
        var currencies = _fixture.CreateMany<CurrencyDto>(3).ToList();
        _currencyRepo.Setup(x => x.GetCurrenciesAsync()).ReturnsAsync(currencies);

        // Act
        var result = await _controller.GetAllCurrencies();

        // Assert
        Assert.Equal(3, result.Value.Count);
        Assert.IsType<ActionResult<List<CurrencyDto>>>(result);
    }

    [Fact]
    public async Task GetCurrency_WithValidCode_ReturnsCurrency()
    {
        // Arrange
        var currency = _fixture.Create<CurrencyDto>();
        _currencyRepo.Setup(x => x.GetCurrencyByCodeAsync(It.IsAny<string>())).ReturnsAsync(currency);

        // Act
        var result = await _controller.GetCurrency(currency.Code);

        // Assert
        Assert.Equal(currency.Name, result.Value.Name);
        Assert.IsType<ActionResult<CurrencyDto>>(result);
    }
    [Fact]
    public async Task GetCurrency_WithInvalidCode_ReturnsNotFound()
    {
        // Arrange
        _currencyRepo.Setup(x => x.GetCurrencyByCodeAsync(It.IsAny<string>())).ReturnsAsync(value: null);

        // Act
        var result = await _controller.GetCurrency("AAA");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCurrency_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        _currencyRepo.Setup(x => x.GetCurrencyByCodeAsync(It.IsAny<string>())).ReturnsAsync(value: null);

        // Act
        var result = await _controller.GetCurrency("");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
    [Fact]
    public async Task CreateCurrency_WithValidCreateCurrencyDto_ReturnsCreatedCurrency()
    {
        // Arrange
        var currency = _fixture.Create<CreateCurrencyDto>();
        _currencyRepo.Setup(x => x.AddCurrency(It.IsAny<Currency>()));
        _currencyRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateCurrency(currency);
        var createdResult = result.Result as CreatedAtActionResult;

        // Assert
        Assert.NotNull(createdResult);
        Assert.Equal("GetCurrency", createdResult.ActionName);
        Assert.IsType<CurrencyDto>(createdResult.Value);
    }
    [Fact]
    public async Task CreateCurrency_FailedSave_ReturnsBadRequest()
    {
        // Arrange
        var currency = _fixture.Create<CreateCurrencyDto>();
        _currencyRepo.Setup(x => x.AddCurrency(It.IsAny<Currency>()));
        _currencyRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        // Act
        var result = await _controller.CreateCurrency(currency);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
    [Fact]
    public async Task UpdateCurrency_WithValidUpdateCurrencyDto_ReturnsOK()
    {
        // Arrange
        var currency = _fixture.Build<Currency>().Without(x => x.Language).Create();
        currency.Name = "Name";
        var updateCurrency = _fixture.Create<UpdateCurrencyDto>();
        _currencyRepo.Setup(x => x.GetCurrencyEntityByCode(It.IsAny<string>())).ReturnsAsync(currency);
        _currencyRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateCurrency(currency.Code, updateCurrency);


        // Assert
        Assert.IsType<OkResult>(result);
    }
    [Fact]
    public async Task DeleteCurrency_WithInvalid_ReturnsOK()
    {
        // Arrange
        var currency = _fixture.Build<Currency>().Without(x => x.Language).Create();

        _currencyRepo.Setup(x => x.GetCurrencyEntityByCode(It.IsAny<string>())).ReturnsAsync(currency);
        _currencyRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);


        // Act
        var result = await _controller.DeleteCurrency(currency.Code);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
