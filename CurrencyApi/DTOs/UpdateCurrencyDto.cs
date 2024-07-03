using System.ComponentModel.DataAnnotations;

namespace CurrencyApi;

public class UpdateCurrencyDto
{
    public string Name { get; set; }
    public string CH_Name { get; set; }
    public decimal Rate { get; set; }
}
