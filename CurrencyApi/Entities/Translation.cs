using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Conventions;

namespace CurrencyApi;

[Table("tbl_Translation")]

public class Translation
{
    public Guid TranslationId { get; set; }
    [Required]
    [MaxLength(8)]
    public string Language { get; set; }
    [Required]
    public string Text { get; set; }
    // nav properties
    public Currency Currency { get; set; }
    public Guid CurrencyId { get; set; }
}
