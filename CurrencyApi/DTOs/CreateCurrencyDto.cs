using System.ComponentModel.DataAnnotations;

namespace CurrencyApi;

public class CreateCurrencyDto
{
    [Required(ErrorMessage = "Code field is required")]
    public string Code { get; set; }
    [Required(ErrorMessage = "Name field is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "CH_Name field is required")]
    public string CH_Name { get; set; }
    [Required(ErrorMessage = "Rate field is required")]
    public decimal Rate { get; set; }
}
