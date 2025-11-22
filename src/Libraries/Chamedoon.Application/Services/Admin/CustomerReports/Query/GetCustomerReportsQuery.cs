using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.CustomerReports.ViewModels;
using Chamedoon.Domin.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.CustomerReports.Query;

public class GetCustomerReportsQuery : IRequest<OperationResult<CustomerReportDashboardVm>>
{
}

public class GetCustomerReportsQueryHandler : IRequestHandler<GetCustomerReportsQuery, OperationResult<CustomerReportDashboardVm>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerReportsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<CustomerReportDashboardVm>> Handle(GetCustomerReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _context.CustomerReports
            .Include(r => r.Customer)
            .ThenInclude(c => c.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var entries = reports.Select(r => new CustomerReportEntryVm
        {
            Id = r.Id,
            CustomerId = r.CustomerId,
            CustomerName = BuildCustomerName(r.Customer?.FirstName, r.Customer?.LastName, r.Customer?.User?.UserName),
            Age = r.Age,
            AgeRange = GetAgeRange(r.Age),
            MaritalStatus = r.MaritalStatus,
            MbtiType = r.MbtiType,
            InvestmentAmount = r.InvestmentAmount,
            JobCategory = r.JobCategory,
            JobTitle = r.JobTitle,
            WorkExperienceYears = r.WorkExperienceYears,
            FieldCategory = r.FieldCategory,
            EducationLevel = r.EducationLevel,
            LanguageCertificate = r.LanguageCertificate,
            WantsFurtherEducation = r.WantsFurtherEducation,
            PhoneNumber = r.PhoneNumber,
            Description = r.Description,
            CreatedAtUtc = r.CreatedAtUtc
        }).ToList();

        var dashboard = new CustomerReportDashboardVm
        {
            Reports = entries,
            AgeDistribution = BuildDistribution(entries.GroupBy(e => e.AgeRange)),
            JobDistribution = BuildDistribution(entries.GroupBy(e => e.JobCategory.GetDescription())),
            EducationDistribution = BuildDistribution(entries.GroupBy(e => e.EducationLevel.GetDescription()))
        };

        return OperationResult<CustomerReportDashboardVm>.Success(dashboard);
    }

    private static IReadOnlyList<DistributionSlice> BuildDistribution(IEnumerable<IGrouping<string, CustomerReportEntryVm>> groups)
    {
        var total = groups.Sum(g => g.Count());
        if (total == 0)
        {
            return Array.Empty<DistributionSlice>();
        }

        return groups
            .Select(g => new DistributionSlice(g.Key, Math.Round((double)g.Count() * 100 / total, 1)))
            .OrderByDescending(x => x.Percent)
            .ToList();
    }

    private static string GetAgeRange(int age)
    {
        if (age < 25) return "کمتر از ۲۵";
        if (age < 35) return "۲۵ تا ۳۴";
        if (age < 45) return "۳۵ تا ۴۴";
        if (age < 55) return "۴۵ تا ۵۴";
        return "۵۵ به بالا";
    }

    private static string BuildCustomerName(string? firstName, string? lastName, string? userName)
    {
        var fullName = ($"{firstName} {lastName}").Trim();
        return string.IsNullOrWhiteSpace(fullName) ? userName ?? "بدون نام" : fullName;
    }
}
