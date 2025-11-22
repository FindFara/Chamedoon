using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.CustomerReports.ViewModels;

public class CustomerReportEntryVm
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int Age { get; set; }
    public string AgeRange { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public string? MbtiType { get; set; }
    public decimal InvestmentAmount { get; set; }
    public JobCategory JobCategory { get; set; }
    public string? JobTitle { get; set; }
    public int WorkExperienceYears { get; set; }
    public string? FieldCategory { get; set; }
    public EducationLevel EducationLevel { get; set; }
    public string? LanguageCertificate { get; set; }
    public bool WantsFurtherEducation { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public record DistributionSlice(string Label, double Percent);

public class CustomerReportDashboardVm
{
    public IReadOnlyList<CustomerReportEntryVm> Reports { get; set; } = Array.Empty<CustomerReportEntryVm>();
    public IReadOnlyList<DistributionSlice> AgeDistribution { get; set; } = Array.Empty<DistributionSlice>();
    public IReadOnlyList<DistributionSlice> JobDistribution { get; set; } = Array.Empty<DistributionSlice>();
    public IReadOnlyList<DistributionSlice> EducationDistribution { get; set; } = Array.Empty<DistributionSlice>();
}
