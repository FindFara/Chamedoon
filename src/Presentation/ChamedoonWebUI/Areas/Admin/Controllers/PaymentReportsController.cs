using Chamedoon.Application.Services.Admin.Payments;
using Chamedoon.Domin.Enums;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class PaymentReportsController : Controller
{
    private readonly IAdminPaymentService _paymentService;

    public PaymentReportsController(IAdminPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<IActionResult> Index(string? search, PaymentStatus? status, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var paymentsResult = await _paymentService.GetPaymentsAsync(search, status, page, pageSize, cancellationToken);
        if (!paymentsResult.IsSuccess || paymentsResult.Result is null)
        {
            return Problem(paymentsResult.Message);
        }

        var model = new PaymentReportIndexViewModel
        {
            Payments = paymentsResult.Result.Items.Select(PaymentReportItemViewModel.FromDto).ToList(),
            SearchTerm = search,
            SelectedStatus = status,
            CurrentPage = paymentsResult.Result.PageNumber,
            TotalPages = paymentsResult.Result.TotalPages,
            PageSize = pageSize,
            TotalCount = paymentsResult.Result.TotalCount
        };

        return View(model);
    }
}
