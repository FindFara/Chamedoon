using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Blogs;

namespace Chamedoon.Application.Services.Blog.ViewModel
{
    public class Article_VM : IMapFrom<Article>
    {
        public long Id { get; set; }
        public string ArticleTitle { get; set; } = string.Empty;
        public string Writer { get; set; }
        public string ArticleDescription { get; set; }
        public string ShortDescription { get; set; }
        public string? ArticleImageName { get; set; } = string.Empty;
    }
}
