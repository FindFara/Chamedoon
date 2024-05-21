using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Article.ViewModel
{
    public class Article_VM
    {
        public required string ArticleTitle { get; set; }
        public required string Writer { get; set; }
        public required string ArticleDescription { get; set; }
        public required string ShortDescription { get; set; }
        public string? ArticleImageName { get; set; } = string.Empty;
    }
}
