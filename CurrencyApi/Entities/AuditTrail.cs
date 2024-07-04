using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApi;


[Table("tbl_AuditTrail")]
[PrimaryKey(nameof(AuditId))]
public class AuditTrail
{
    public Guid AuditId { get; set; }= Guid.NewGuid();
    [MaxLength(100)]
    public string Action { get; set; }
    [MaxLength(100)]
    public string Controller { get; set; }
    [MaxLength(10)]
    public string Method { get; set; }
    [MaxLength(256)]
    public string RequestContentType { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    [MaxLength(5)]
    public string IsAuthenticated { get; set; }
    [MaxLength(5)]
    public string ModelState { get; set; }
    [MaxLength(256)]
    public string ResponseContentType { get; set; }
    [MaxLength(100)]
    public string ClientIP { get; set; }
    public int HttpStatus { get; set; }
    public DateTime ReqeustDateUTC { get; set; } = DateTime.UtcNow;
    public DateTime ResponseDateUTC { get; set; } = DateTime.UtcNow;
}
