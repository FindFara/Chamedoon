using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Blogs;

namespace Chamedoon.Application.Services.Blog.ViewModel;
public class BlogViewModel :IMapFrom<Article>
{
    public long Id { get; set; }
    public string ArticleTitle { get; set; }
    public string Writer { get; set; }
    public long VisitCount { get; set; }
    public string ShortDescription { get; set; }
    public DateTime Created { get; set; }
    public string? ArticleImageName { get; set; } = string.Empty;
}
