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
        var now = DateTime.UtcNow;
        var start = new DateTime(now.Year, now.Month, 1).AddMonths(-(months - 1));

        var data = await _context.Article
            .Where(article => article.Created >= start)
            .GroupBy(article => new { article.Created.Year, article.Created.Month })
            .Select(group => new MonthlyRegistrationCount(group.Key.Year, group.Key.Month, group.Sum(article => (int)article.VisitCount)))
            .ToListAsync(cancellationToken);

        var results = new List<MonthlyRegistrationCount>();
        for (var i = 0; i < months; i++)
        {
            var date = start.AddMonths(i);
            var match = data.FirstOrDefault(record => record.Year == date.Year && record.Month == date.Month);
            results.Add(match ?? new MonthlyRegistrationCount(date.Year, date.Month, 0));
        }

        return results;
    }
}
