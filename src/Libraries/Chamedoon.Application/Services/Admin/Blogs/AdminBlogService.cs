using System;
using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Blogs;

namespace Chamedoon.Application.Services.Admin.Blogs;

public class AdminBlogService : IAdminBlogService
{
    private readonly IAdminBlogRepository _blogRepository;

    public AdminBlogService(IAdminBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<OperationResult<IReadOnlyList<AdminBlogPostDto>>> GetPostsAsync(string? search, CancellationToken cancellationToken)
    {
        var posts = await _blogRepository.GetArticlesAsync(search, cancellationToken);
        var mapped = posts.Select(article => article.ToAdminBlogPostDto()).ToList();
        return OperationResult<IReadOnlyList<AdminBlogPostDto>>.Success(mapped);
    }

    public async Task<OperationResult<AdminBlogPostDto>> GetPostAsync(long id, CancellationToken cancellationToken)
    {
        var article = await _blogRepository.GetArticleAsync(id, cancellationToken);
        if (article is null)
        {
            return OperationResult<AdminBlogPostDto>.Fail("مقاله مورد نظر یافت نشد.");
        }

        return OperationResult<AdminBlogPostDto>.Success(article.ToAdminBlogPostDto());
    }

    public async Task<OperationResult<AdminBlogPostDto>> CreatePostAsync(AdminBlogPostInput input, CancellationToken cancellationToken)
    {
        var article = BuildArticle(input);
        article.Created = DateTime.Now;
        article.LastModified = DateTime.Now;

        var created = await _blogRepository.CreateArticleAsync(article, cancellationToken);
        return OperationResult<AdminBlogPostDto>.Success(created.ToAdminBlogPostDto());
    }

    public async Task<OperationResult<AdminBlogPostDto>> UpdatePostAsync(AdminBlogPostInput input, CancellationToken cancellationToken)
    {
        if (!input.Id.HasValue)
        {
            return OperationResult<AdminBlogPostDto>.Fail("شناسه مقاله ارسال نشده است.");
        }

        var article = BuildArticle(input);
        article.Id = input.Id.Value;
        article.LastModified = DateTime.Now;

        var updated = await _blogRepository.UpdateArticleAsync(article, cancellationToken);
        if (updated is null)
        {
            return OperationResult<AdminBlogPostDto>.Fail("مقاله مورد نظر یافت نشد.");
        }

        return OperationResult<AdminBlogPostDto>.Success(updated.ToAdminBlogPostDto());
    }

    public async Task<OperationResult<bool>> DeletePostAsync(long id, CancellationToken cancellationToken)
    {
        var deleted = await _blogRepository.DeleteArticleAsync(id, cancellationToken);
        if (!deleted)
        {
            return OperationResult<bool>.Fail("امکان حذف مقاله وجود ندارد.");
        }

        return OperationResult<bool>.Success(true);
    }

    private static Article BuildArticle(AdminBlogPostInput input)
        => new()
        {
            ArticleTitle = input.Title,
            Writer = input.Writer,
            ShortDescription = input.ShortDescription,
            ArticleDescription = input.ArticleDescription,
            ArticleImageName = input.ImageName,
            VisitCount = input.VisitCount
        };
}
