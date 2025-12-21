using Chamedoon.Domin.Base;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Payments;

public class DiscountCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }
    public string? Description { get; set; }
}
