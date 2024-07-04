﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CurrencyApi;
[ApiController]
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyDBContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CurrencyController> _localizer;

    public CurrencyController(CurrencyDBContext context, IMapper mapper, IStringLocalizer<CurrencyController> localizer)
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }
    [HttpGet]
    public async Task<ActionResult<List<CurrenciesDto>>> GetAllCurrencies()
    {
        var query = _context.Currencies.OrderBy(x => x.Code).AsQueryable();
        if (!query.Any()) return NotFound();
        return await query.ProjectTo<CurrenciesDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
    [HttpGet("{code}")]
    public async Task<ActionResult<CurrenciesDto>> GetCurrency(string code)
    {
        if(string.IsNullOrEmpty(code)) return BadRequest(_localizer["Code field is required"].Value);
        var query = await _context.Currencies.Include(x => x.Language).FirstOrDefaultAsync(x => x.Code == code);
        if (query == null) return BadRequest(_localizer["Invalid Code"].Value);
        return _mapper.Map<CurrenciesDto>(query);
    }
    [HttpPost]
    public async Task<ActionResult<CurrenciesDto>> CreateCurrency(CreateCurrencyDto newCurrency)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var currency = _mapper.Map<Currency>(newCurrency);
        currency.UpdatedUTC = DateTime.UtcNow;
        currency.Language.Add(new Translation()
        {
            Language = "zh-TW",
            Text = newCurrency.CH_Name
        });
        _context.Currencies.Add(currency);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest(_localizer["Database update failed"].Value);

        return CreatedAtAction(nameof(GetCurrency), new { currency.Code }, _mapper.Map<CurrenciesDto>(currency));
    }

    [HttpPut("{code}")]
    public async Task<ActionResult<CurrenciesDto>> UpdateCurrency(string code, UpdateCurrencyDto updateCurrency)
    {
        if(string.IsNullOrEmpty(code.Trim())) return BadRequest(_localizer["Code field is required"].Value);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var currency = await _context.Currencies.Include(x => x.Language).FirstOrDefaultAsync(x => x.Code == code);
        if (currency == null) return BadRequest(_localizer["Invalid Code"].Value);

        currency.Name = String.IsNullOrEmpty(updateCurrency.Name) ? currency.Name : updateCurrency.Name;
        currency.Rate = updateCurrency.Rate == 0 ? currency.Rate : updateCurrency.Rate;
        currency.UpdatedUTC = DateTime.UtcNow;
        if (!String.IsNullOrEmpty(updateCurrency.CH_Name))
        {
            currency.Language.Remove(currency.Language.FirstOrDefault(x => x.Language == "zh-TW"));
            currency.Language.Add(new Translation()
            {
                Language = "zh-TW",
                Text = updateCurrency.CH_Name
            });
        }

        var result = await _context.SaveChangesAsync() > 0;
        if (result) return Ok();
        return BadRequest(_localizer["Database update failed"].Value);
    }
    [HttpDelete("{code}")]
    public async Task<ActionResult> DeleteCurrency(string code)
    {
        if(string.IsNullOrEmpty(code)) return BadRequest(_localizer["Code field is required"].Value);
        var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Code == code);
        if (currency == null) return BadRequest(_localizer["Invalid Code"].Value);
        _context.Currencies.Remove(currency);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result) return BadRequest(_localizer["Database update failed"].Value);
        return Ok();
    }
}
