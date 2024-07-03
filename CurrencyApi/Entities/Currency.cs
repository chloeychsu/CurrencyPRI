using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;

[Table("tbl_Currencies")]
[PrimaryKey(nameof(CurrencyId))]
[Index(nameof(Code))]

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
    [Required]
    public DateTime UpdatedUTC { get; set; } = DateTime.UtcNow;
    public ICollection<Translation> Language { get; set; } = new List<Translation>();
}
