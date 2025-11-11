using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Blogs;

public interface IAdminBlogService
{
    Task<OperationResult<IReadOnlyList<AdminBlogPostDto>>> GetPostsAsync(string? search, CancellationToken cancellationToken);
    Task<OperationResult<AdminBlogPostDto>> GetPostAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<AdminBlogPostDto>> CreatePostAsync(AdminBlogPostInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminBlogPostDto>> UpdatePostAsync(AdminBlogPostInput input, CancellationToken cancellationToken);
    Task<OperationResult<bool>> DeletePostAsync(long id, CancellationToken cancellationToken);
}
