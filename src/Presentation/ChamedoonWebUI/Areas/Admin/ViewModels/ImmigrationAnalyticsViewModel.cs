using System.Collections.Generic;

namespace ChamedoonWebUI.Areas.Admin.ViewModels
{
    public class ImmigrationAnalyticsViewModel
    {
        public IReadOnlyList<DistributionItemViewModel> AgeDistribution { get; init; } = new List<DistributionItemViewModel>();
        public IReadOnlyList<DistributionItemViewModel> JobDistribution { get; init; } = new List<DistributionItemViewModel>();
        public IReadOnlyList<DistributionItemViewModel> DegreeDistribution { get; init; } = new List<DistributionItemViewModel>();
        public int TotalEvaluations { get; init; }
    }

    public record DistributionItemViewModel(string Label, double Percentage, int Count);
}
