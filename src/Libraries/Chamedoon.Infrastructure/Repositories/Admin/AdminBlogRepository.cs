using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminBlogRepository : IAdminBlogRepository
{
    private readonly ApplicationDbContext _context;

    public AdminBlogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Article>> GetArticlesAsync(string? search, CancellationToken cancellationToken)
    {
        IQueryable<Article> query = _context.Article.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(article =>
                article.ArticleTitle.Contains(term) ||
                article.Writer.Contains(term));
        }

        return await query
            .AsNoTracking()
            .OrderByDescending(article => article.Created)
            .ToListAsync(cancellationToken);
    }

    public Task<Article?> GetArticleAsync(long id, CancellationToken cancellationToken)
        => _context.Article
            .AsNoTracking()
            .FirstOrDefaultAsync(article => article.Id == id, cancellationToken);

    public async Task<Article> CreateArticleAsync(Article article, CancellationToken cancellationToken)
    {
        await _context.Article.AddAsync(article, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return article;
    }

    public async Task<Article?> UpdateArticleAsync(Article article, CancellationToken cancellationToken)
    {
        var existing = await _context.Article.FirstOrDefaultAsync(a => a.Id == article.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.ArticleTitle = article.ArticleTitle;
        existing.Writer = article.Writer;
        existing.ShortDescription = article.ShortDescription;
        existing.ArticleDescription = article.ArticleDescription;
        existing.ArticleImageName = article.ArticleImageName;
        existing.VisitCount = article.VisitCount;
        existing.LastModified = article.LastModified ?? DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteArticleAsync(long id, CancellationToken cancellationToken)
    {
        var article = await _context.Article.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (article is null)
        {
            return false;
        }

        _context.Article.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<int> CountArticlesAsync(CancellationToken cancellationToken)
        => _context.Article.CountAsync(cancellationToken);

    public Task<int> CountPublishedArticlesAsync(CancellationToken cancellationToken)
        => _context.Article.CountAsync(article => article.VisitCount > 0, cancellationToken);

    public Task<long> SumArticleViewsAsync(CancellationToken cancellationToken)
        => _context.Article.SumAsync(article => article.VisitCount, cancellationToken);

    public async Task<List<Article>> GetTopArticlesAsync(int count, CancellationToken cancellationToken)
    {
        return await _context.Article
            .AsNoTracking()
            .OrderByDescending(article => article.VisitCount)
            .ThenByDescending(article => article.Created)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Article>> GetRecentArticlesAsync(int count, CancellationToken cancellationToken)
        => _context.Article
            .AsNoTracking()
            .OrderByDescending(article => article.Created)
            .Take(count)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<MonthlyRegistrationCount>> GetMonthlyArticleViewCountsAsync(int months, CancellationToken cancellationToken)
    {
        var articleViewSnapshots = await _context.Article
            .Select(article => new
            {
                Date = article.LastModified ?? article.Created,
                article.VisitCount
            })
            .ToListAsync(cancellationToken);

        if (!articleViewSnapshots.Any())
        {
            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1).AddMonths(-(months - 1));
            return Enumerable.Range(0, months)
                .Select(offset =>
                {
                    var current = start.AddMonths(offset);
                    return new MonthlyRegistrationCount(current.Year, current.Month, 0);
                })
                .ToList();
        }

        var latest = articleViewSnapshots.Max(snapshot => new DateTime(snapshot.Date.Year, snapshot.Date.Month, 1));
        var windowStart = latest.AddMonths(-(months - 1));

        var grouped = articleViewSnapshots
            .GroupBy(snapshot => new { snapshot.Date.Year, snapshot.Date.Month })
            .Select(group => new MonthlyRegistrationCount(group.Key.Year, group.Key.Month, group.Sum(article => (int)article.VisitCount)))
            .ToDictionary(record => (record.Year, record.Month), record => record.Count);

        var results = new List<MonthlyRegistrationCount>();
        for (var i = 0; i < months; i++)
        {
            var date = windowStart.AddMonths(i);
            var key = (date.Year, date.Month);
            var count = grouped.TryGetValue(key, out var value) ? value : 0;
            results.Add(new MonthlyRegistrationCount(date.Year, date.Month, count));
        }

        return results;
    }
}
