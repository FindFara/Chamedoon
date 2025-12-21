using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminDiscountCodeDto(
    long Id,
    string Code,
    DiscountType Type,
    int Value,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? ExpiresAtUtc,
    string? Description);

public record AdminDiscountCodeInput(
    long? Id,
    string Code,
    DiscountType Type,
    int Value,
    bool IsActive,
    DateTime? ExpiresAtUtc,
    string? Description);
