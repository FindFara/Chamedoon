using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Immigration;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class ImmigrationReportsController : Controller
{
    private readonly IImmigrationEvaluationService _evaluationService;

    public ImmigrationReportsController(IImmigrationEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var analytics = await _evaluationService.GetAnalyticsAsync(cancellationToken);

        var viewModel = new ImmigrationAnalyticsViewModel
        {
            AgeDistribution = analytics.AgeDistribution
                .Select(item => new DistributionItemViewModel(item.Label, item.Percentage, item.Count))
                .ToList(),
            JobDistribution = analytics.JobDistribution
                .Select(item => new DistributionItemViewModel(item.Label, item.Percentage, item.Count))
                .ToList(),
            DegreeDistribution = analytics.DegreeDistribution
                .Select(item => new DistributionItemViewModel(item.Label, item.Percentage, item.Count))
                .ToList(),
            TotalEvaluations = analytics.TotalEvaluations
        };

        return View(viewModel);
    }
}
