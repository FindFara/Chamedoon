using System.Collections.Generic;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class PaymentReportIndexViewModel
{
    public List<PaymentReportItemViewModel> Payments { get; set; } = new();
    public string? SearchTerm { get; set; }
    public PaymentStatus? SelectedStatus { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? UserName { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public class PaymentReportItemViewModel
{
    public long Id { get; set; }
    public string? PlanId { get; set; }
    public int Amount { get; set; }
    public int FinalAmount { get; set; }
    public int? DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PaidAtUtc { get; set; }
    public string? GatewayTrackId { get; set; }
    public string? ReferenceCode { get; set; }
    public string? Description { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }

    public string StatusLabel => Status switch
    {
        PaymentStatus.Paid => "موفق",
        PaymentStatus.Pending => "در انتظار",
        PaymentStatus.Redirected => "در انتظار",
        PaymentStatus.Failed => "ناموفق",
        PaymentStatus.Cancelled => "لغو شده",
        _ => "نامشخص"
    };

    public string StatusBadgeClass => Status switch
    {
        PaymentStatus.Paid => "bg-success",
        PaymentStatus.Pending => "bg-warning text-dark",
        PaymentStatus.Redirected => "bg-warning text-dark",
        PaymentStatus.Failed => "bg-danger",
        PaymentStatus.Cancelled => "bg-secondary",
        _ => "bg-light text-dark"
    };

    public static PaymentReportItemViewModel FromDto(AdminPaymentDto dto)
        => new()
        {
            Id = dto.Id,
            PlanId = dto.PlanId,
            Amount = dto.Amount,
            FinalAmount = dto.FinalAmount,
            DiscountAmount = dto.DiscountAmount,
            DiscountCode = dto.DiscountCode,
            Status = dto.Status,
            CreatedAtUtc = dto.CreatedAtUtc,
            PaidAtUtc = dto.PaidAtUtc,
            GatewayTrackId = dto.GatewayTrackId,
            ReferenceCode = dto.ReferenceCode,
            Description = dto.Description,
            CustomerName = dto.CustomerName,
            UserName = dto.UserName,
            Email = dto.Email
        };
}
