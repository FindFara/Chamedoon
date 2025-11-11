using System.Collections.Generic;
using Chamedoon.Domin.Entity.Blogs;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminBlogRepository
{
    Task<List<Article>> GetArticlesAsync(string? search, CancellationToken cancellationToken);
    Task<Article?> GetArticleAsync(long id, CancellationToken cancellationToken);
    Task<Article> CreateArticleAsync(Article article, CancellationToken cancellationToken);
    Task<Article?> UpdateArticleAsync(Article article, CancellationToken cancellationToken);
    Task<bool> DeleteArticleAsync(long id, CancellationToken cancellationToken);
    Task<int> CountArticlesAsync(CancellationToken cancellationToken);
    Task<int> CountPublishedArticlesAsync(CancellationToken cancellationToken);
    Task<long> SumArticleViewsAsync(CancellationToken cancellationToken);
    Task<List<Article>> GetTopArticlesAsync(int count, CancellationToken cancellationToken);
    Task<List<Article>> GetRecentArticlesAsync(int count, CancellationToken cancellationToken);
}
