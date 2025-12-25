using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Immigration;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ImmigrationReportsController : Controller
{
    private readonly IImmigrationEvaluationService _evaluationService;

    public ImmigrationReportsController(IImmigrationEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    public async Task<IActionResult> Index(string? query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var analytics = await _evaluationService.GetAnalyticsAsync(cancellationToken);
        var evaluations = await _evaluationService.SearchAsync(query, page, pageSize, cancellationToken);

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
            Evaluations = evaluations.Items
                .Select(item => new ImmigrationEvaluationItemViewModel(
                    item.Id,
                    item.CustomerName,
                    item.Email,
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
            TotalEvaluations = analytics.TotalEvaluations,
            CurrentPage = evaluations.PageNumber,
            TotalPages = evaluations.TotalPages,
            PageSize = pageSize,
            TotalCount = evaluations.TotalCount
        };

        return View(viewModel);
    }
}
