using System.Collections.Generic;

namespace ChamedoonWebUI.Areas.Admin.ViewModels
{
    public class ImmigrationAnalyticsViewModel
    {
        public IReadOnlyList<DistributionItemViewModel> AgeDistribution { get; init; } = new List<DistributionItemViewModel>();
        public IReadOnlyList<DistributionItemViewModel> JobDistribution { get; init; } = new List<DistributionItemViewModel>();
        public IReadOnlyList<DistributionItemViewModel> DegreeDistribution { get; init; } = new List<DistributionItemViewModel>();
        public IReadOnlyList<ImmigrationEvaluationItemViewModel> Evaluations { get; init; } = new List<ImmigrationEvaluationItemViewModel>();
        public string? Query { get; init; }
        public int TotalEvaluations { get; init; }
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
    }

    public record DistributionItemViewModel(string Label, double Percentage, int Count);
    public record ImmigrationEvaluationItemViewModel(
        long Id,
        string CustomerName,
        string? PhoneNumber,
        int Age,
        string MaritalStatus,
        string JobCategory,
        string? JobTitle,
        string DegreeLevel,
        string LanguageCertificate,
        bool WillingToStudy,
        DateTime CreatedAtUtc);
}
