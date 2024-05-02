using Chamedoon.Domin.Base;

namespace CodeTo.Domain.Entities.Articles;
public class Article : BaseAuditableEntity
{
    public required string ArticleTitle { get; set; }
    public required string Writer { get; set; }
    public required string ArticleDescription { get; set; }
    public required string ShortDescription { get; set; }
    public string? ArticleImageName { get; set; } = string.Empty;
    public long VisitCount { get; set; } = default;

    #region Relations
    public List<ArticleComment> ArticleComment { get; set; }
    #endregion

}
