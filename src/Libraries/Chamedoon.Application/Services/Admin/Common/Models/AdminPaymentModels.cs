using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminPaymentDto(
    long Id,
    string? PlanId,
    int Amount,
    int FinalAmount,
    string? DiscountCode,
    int? DiscountAmount,
    PaymentStatus Status,
    DateTime CreatedAtUtc,
    DateTime? PaidAtUtc,
    string? GatewayTrackId,
    string? ReferenceCode,
    string? Description,
    string CustomerName,
    string? UserName,
    string? Email);
