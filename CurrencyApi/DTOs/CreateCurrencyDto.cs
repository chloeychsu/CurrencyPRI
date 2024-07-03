using System.ComponentModel.DataAnnotations;

namespace CurrencyApi;

public class CreateCurrencyDto
{
    [Required]
    public string Code { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string CH_Name { get; set; }
    [Required]
    public decimal Rate { get; set; }
}
