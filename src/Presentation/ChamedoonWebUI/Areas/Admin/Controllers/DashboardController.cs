using Chamedoon.Application.Services.Admin.Dashboard;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly IAdminDashboardService _dashboardService;

    public DashboardController(IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var summaryResult = await _dashboardService.GetSummaryAsync(cancellationToken);
        if (!summaryResult.IsSuccess || summaryResult.Result is null)
        {
            return Problem(summaryResult.Message);
        }

        var viewModel = DashboardViewModel.FromDto(summaryResult.Result);
        return View(viewModel);
    }
}
