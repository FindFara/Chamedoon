using ChamedoonWebUI.Areas.Admin.ViewModels;
using ChamedoonWebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly IAdminDataService _dataService;

    public DashboardController(IAdminDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index()
    {
        var summary = _dataService.GetDashboardSummary();
        var roles = _dataService.GetRoles().ToDictionary(r => r.Id, r => r.Name);

        var model = new DashboardViewModel
        {
            Summary = summary,
            RoleLookup = roles
        };

        return View(model);
    }
}
