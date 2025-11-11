namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminBlogPostDto(
    long Id,
    string Title,
    string Writer,
    string ShortDescription,
    string ArticleDescription,
    string? ImageName,
    long VisitCount,
    DateTime CreatedAt,
    DateTime? LastModifiedAt);

public class AdminBlogPostInput
{
    public long? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Writer { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ArticleDescription { get; set; } = string.Empty;
    public string? ImageName { get; set; }
    public long VisitCount { get; set; }
}
