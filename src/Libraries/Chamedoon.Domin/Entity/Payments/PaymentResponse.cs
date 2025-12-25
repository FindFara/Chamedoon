using Chamedoon.Domin.Base;

namespace Chamedoon.Domin.Entity.Payments;

public class PaymentResponse : BaseEntity
{
    public long PaymentRequestId { get; set; }
    public PaymentRequest PaymentRequest { get; set; } = null!;
    public string Type { get; set; } = string.Empty;
    public int ResultCode { get; set; }
    public string? Message { get; set; }
    public string? RawPayload { get; set; }
    public string? GatewayTrackId { get; set; }
    public string? ReferenceId { get; set; }
    public string? CardNumber { get; set; }
    public int? Amount { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.Now;
    public DateTime? PaidAtUtc { get; set; }
}

