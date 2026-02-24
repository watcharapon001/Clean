using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.SU;

public class SuConfig : BaseAuditableEntity
{
    [Key]
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DataType { get; set; } = "text"; // "text", "number", "boolean"
}
