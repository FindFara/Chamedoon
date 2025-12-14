using System;
using System.Collections.Generic;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Payments;

public class PaymentGatewayOptions
{
    public const string SectionName = "PaymentGateway";
    public const string HttpClientName = "ZibalGateway";

    public string MerchantId { get; set; } = "zibal";
    public string BaseUrl { get; set; } = "https://sandbox.zibal.ir/v1/";
    public string StartUrl { get; set; } = "https://sandbox.zibal.ir/start";
    public string CallbackPath { get; set; } = "/payment/callback";
    public int AmountMultiplier { get; set; } = 10;
}

public record PaymentRedirectResult
{
    public bool IsSuccess { get; init; }
    public string? RedirectUrl { get; init; }
    public string? ErrorMessage { get; init; }
    public long? PaymentRequestId { get; init; }

    public static PaymentRedirectResult Success(string redirectUrl, long requestId)
        => new() { IsSuccess = true, RedirectUrl = redirectUrl, PaymentRequestId = requestId };

    public static PaymentRedirectResult Failure(string message)
        => new() { IsSuccess = false, ErrorMessage = message };
}

public record PaymentVerificationResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public PaymentStatus Status { get; init; }
    public long? PaymentRequestId { get; init; }
    public string? PlanId { get; init; }
    public long? CustomerId { get; init; }

    public static PaymentVerificationResult FromStatus(PaymentStatus status, string message, long? requestId, string? planId, long? customerId)
        => new() { IsSuccess = status == PaymentStatus.Paid, Message = message, Status = status, PaymentRequestId = requestId, PlanId = planId, CustomerId = customerId };
}

public record PlanDiscountPreview
{
    public string PlanId { get; init; } = string.Empty;
    public string PlanTitle { get; init; } = string.Empty;
    public int BaseAmount { get; init; }
    public int DiscountAmount { get; init; }
    public int FinalAmount { get; init; }
}

public record DiscountPreviewResult
{
    public bool IsValid { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Code { get; init; }
    public IReadOnlyList<PlanDiscountPreview> Plans { get; init; } = Array.Empty<PlanDiscountPreview>();

    public static DiscountPreviewResult Invalid(string message, IReadOnlyList<PlanDiscountPreview> plans)
        => new() { IsValid = false, Message = message, Plans = plans };

    public static DiscountPreviewResult Valid(string code, string message, IReadOnlyList<PlanDiscountPreview> plans)
        => new() { IsValid = true, Message = message, Code = code, Plans = plans };
}

