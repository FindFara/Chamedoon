using System;
using System.Collections.Generic;

namespace ChamedoonWebUI.Models
{
    public class BlogSearchFormModel
    {
        public string FormIdPrefix { get; set; } = "blog";
        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = "Blog";
        public string SubmitLabel { get; set; } = "جستجو";
        public string KeywordLabel { get; set; } = "کلمه کلیدی";
        public string WriterLabel { get; set; } = "نویسنده";
        public string KeywordPlaceholder { get; set; } = "عنوان یا متن مقاله...";
        public string WriterPlaceholder { get; set; } = "نام نویسنده";
        public string SubmitButtonClass { get; set; } = "btn-glass-primary";
        public string? Search { get; set; }
        public string? Writer { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<string>? WriterOptions { get; set; }
        public bool ShowReset { get; set; }
    }
}
