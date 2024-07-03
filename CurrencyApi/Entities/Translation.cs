using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyApi;

[Table("tbl_Translation")]

public class Translation
{
    public Guid TranslationId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Table { get; set; }
    [Required]
    public Guid RelationId { get; set; }
    [Required]
    [MaxLength(8)]
    public string Language { get; set; }
    [Required]
    public string Text { get; set; }
}
