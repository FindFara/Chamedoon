using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Customers;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Immigration
{
    public interface IImmigrationEvaluationService
    {
        Task RecordAsync(ImmigrationInput input, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<ImmigrationAnalyticsResult> GetAnalyticsAsync(CancellationToken cancellationToken);
        Task<PaginatedList<ImmigrationEvaluationListItem>> SearchAsync(string? query, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }

    public record DistributionItem(string Label, double Percentage, int Count);

    public class ImmigrationAnalyticsResult
    {
        public List<DistributionItem> AgeDistribution { get; init; } = new();
        public List<DistributionItem> JobDistribution { get; init; } = new();
        public List<DistributionItem> DegreeDistribution { get; init; } = new();
        public int TotalEvaluations { get; init; }
    }

    public record ImmigrationEvaluationListItem(
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

    public class ImmigrationEvaluationService : IImmigrationEvaluationService
    {
        private readonly IApplicationDbContext _context;

        public ImmigrationEvaluationService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RecordAsync(ImmigrationInput input, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserId(user);
            if (!userId.HasValue)
            {
                return;
            }

            var entity = new ImmigrationEvaluation
            {
                CustomerId = userId.Value,
                Age = input.Age,
                MaritalStatus = (int)input.MaritalStatus,
                MBTIPersonality = (int)input.MBTIPersonality,
                InvestmentAmount = input.InvestmentAmount,
                JobCategory = (int)input.JobCategory,
                JobTitle = input.JobTitle,
                WorkExperienceYears = input.WorkExperienceYears,
                FieldCategory = (int)input.FieldCategory,
                DegreeLevel = (int)input.DegreeLevel,
                LanguageCertificate = (int)input.LanguageCertificate,
                WillingToStudy = input.WillingToStudy,
                PhoneNumber = input.PhoneNumber,
                Notes = input.Notes,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _context.ImmigrationEvaluations.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ImmigrationAnalyticsResult> GetAnalyticsAsync(CancellationToken cancellationToken)
        {
            var evaluations = await _context.ImmigrationEvaluations
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var total = evaluations.Count;
            var ageDistribution = CalculateAgeDistribution(evaluations, total);
            var jobDistribution = CalculateDistribution(
                evaluations,
                total,
                evaluation => ((JobCategoryType)evaluation.JobCategory).ToDisplay(),
                category => category.JobCategory);

            var degreeDistribution = CalculateDistribution(
                evaluations,
                total,
                evaluation => ((DegreeLevelType)evaluation.DegreeLevel).ToDisplay(),
                degree => degree.DegreeLevel);

            return new ImmigrationAnalyticsResult
            {
                AgeDistribution = ageDistribution,
                JobDistribution = jobDistribution,
                DegreeDistribution = degreeDistribution,
                TotalEvaluations = total
            };
        }

        public async Task<PaginatedList<ImmigrationEvaluationListItem>> SearchAsync(string? query, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var normalizedQuery = query?.Trim();

            var evaluations = _context.ImmigrationEvaluations
                .AsNoTracking()
                .Include(item => item.Customer)
                .OrderByDescending(item => item.CreatedAtUtc);

            if (!string.IsNullOrWhiteSpace(normalizedQuery))
            {
                var lowered = normalizedQuery.ToLower();
                evaluations = evaluations.Where(item =>
                    (!string.IsNullOrWhiteSpace(item.Customer.FirstName) && item.Customer.FirstName.ToLower().Contains(lowered)) ||
                    (!string.IsNullOrWhiteSpace(item.Customer.LastName) && item.Customer.LastName.ToLower().Contains(lowered)) ||
                    (!string.IsNullOrWhiteSpace(item.PhoneNumber) && item.PhoneNumber.ToLower().Contains(lowered)) ||
                    (!string.IsNullOrWhiteSpace(item.JobTitle) && item.JobTitle.ToLower().Contains(lowered)))
                    .OrderByDescending(item => item.CreatedAtUtc); ;
            }

            var paginated = await PaginatedList<ImmigrationEvaluation>.CreateAsync(evaluations, pageNumber, pageSize);
            var mappedItems = paginated.Items
                .Select(item => new ImmigrationEvaluationListItem(
                    item.Id,
                    BuildCustomerName(item.Customer),
                    item.PhoneNumber,
                    item.Age,
                    ((MaritalStatusType)item.MaritalStatus).ToDisplay(),
                    ((JobCategoryType)item.JobCategory).ToDisplay(),
                    item.JobTitle,
                    ((DegreeLevelType)item.DegreeLevel).ToDisplay(),
                    ((LanguageCertificateType)item.LanguageCertificate).ToDisplay(),
                    item.WillingToStudy,
                    item.CreatedAtUtc))
                .ToList();

            return new PaginatedList<ImmigrationEvaluationListItem>(mappedItems, paginated.TotalCount, paginated.PageNumber, pageSize);
        }

        private static List<DistributionItem> CalculateAgeDistribution(IEnumerable<ImmigrationEvaluation> evaluations, int total)
        {
            var buckets = new Dictionary<string, int>
            {
                { "زیر ۲۵ سال", 0 },
                { "۲۵ تا ۳۴ سال", 0 },
                { "۳۵ تا ۴۴ سال", 0 },
                { "۴۵ تا ۵۴ سال", 0 },
                { "۵۵ سال و بیشتر", 0 },
                { "نامشخص", 0 }
            };

            foreach (var evaluation in evaluations)
            {
                var age = evaluation.Age;
                var bucket = age switch
                {
                    < 1 => "نامشخص",
                    < 25 => "زیر ۲۵ سال",
                    >= 25 and <= 34 => "۲۵ تا ۳۴ سال",
                    >= 35 and <= 44 => "۳۵ تا ۴۴ سال",
                    >= 45 and <= 54 => "۴۵ تا ۵۴ سال",
                    _ => "۵۵ سال و بیشتر"
                };

                buckets[bucket]++;
            }

            return ToDistribution(buckets, total);
        }

        private static List<DistributionItem> CalculateDistribution<T>(
            IEnumerable<ImmigrationEvaluation> evaluations,
            int total,
            Func<ImmigrationEvaluation, string> labelSelector,
            Func<ImmigrationEvaluation, T> groupingKey) where T : notnull
        {
            var counts = evaluations
                .GroupBy(groupingKey)
                .ToDictionary(group => labelSelector(group.First()), group => group.Count());

            return ToDistribution(counts, total);
        }

        private static List<DistributionItem> ToDistribution(Dictionary<string, int> source, int total)
        {
            return source
                .Select(kvp => new DistributionItem(
                    kvp.Key,
                    total == 0 ? 0 : Math.Round((double)kvp.Value / total * 100, 1),
                    kvp.Value))
                .OrderByDescending(item => item.Percentage)
                .ToList();
        }

        private static long? GetUserId(ClaimsPrincipal user)
        {
            var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name;
            return long.TryParse(idValue, out var id) ? id : null;
        }

        private static string BuildCustomerName(Customer customer)
        {
            var parts = new[] { customer.FirstName, customer.LastName }
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();

            return parts.Length > 0 ? string.Join(" ", parts) : "نامشخص";
        }
    }
}
