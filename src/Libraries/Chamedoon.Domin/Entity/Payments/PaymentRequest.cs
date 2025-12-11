using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Domin.Entity.Payments;

public class PaymentRequest : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string? PlanId { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public string? CallbackUrl { get; set; }
    public string? GatewayTrackId { get; set; }
    public string? ReferenceCode { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? PaymentUrl { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAtUtc { get; set; }
    public string? LastError { get; set; }
    public ICollection<PaymentResponse> Responses { get; set; } = new List<PaymentResponse>();
}

