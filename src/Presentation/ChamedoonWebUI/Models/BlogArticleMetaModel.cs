namespace ChamedoonWebUI.Models
{
    public class BlogArticleMetaModel
    {
        public string Writer { get; set; } = string.Empty;

        public string VisitCountText { get; set; } = string.Empty;

        public string VisitLabel { get; set; } = "تعداد بازدید";

        public string VisitCaption { get; set; } = "بازدید";

        public string ShareUrl { get; set; } = string.Empty;

        public string? ShareTitle { get; set; }

        public string? ShareText { get; set; }

        public string ShareAriaLabel { get; set; } = "اشتراک‌گذاری مقاله";

        public string ShareSuccessLabel { get; set; } = "لینک کپی شد";

        public string ShareErrorLabel { get; set; } = "امکان اشتراک نیست";

        public bool ShowShareButton => !string.IsNullOrWhiteSpace(ShareUrl);
    }
}
