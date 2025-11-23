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

    public async Task<IActionResult> Index(string? query, CancellationToken cancellationToken)
    {
        var analytics = await _evaluationService.GetAnalyticsAsync(cancellationToken);
        var evaluations = await _evaluationService.SearchAsync(query, cancellationToken);

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
            Evaluations = evaluations
                .Select(item => new ImmigrationEvaluationItemViewModel(
                    item.Id,
                    item.CustomerName,
                    item.PhoneNumber,
                    item.Age,
                    item.MaritalStatus,
                    item.JobCategory,
                    item.JobTitle,
                    item.DegreeLevel,
                    item.LanguageCertificate,
                    item.WillingToStudy,
                    item.CreatedAtUtc))
                .ToList(),
            Query = query,
            TotalEvaluations = analytics.TotalEvaluations
        };

        return View(viewModel);
    }
}
