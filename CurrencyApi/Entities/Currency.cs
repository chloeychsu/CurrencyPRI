using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyApi;

[Table("tbl_Currency")]
public class Currency
{
    public Guid CurrencyId { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(3)]
    public string Code { get; set; }
    [Required]
    [MaxLength(256)]
    public string Name { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Rate { get; set; }
}
