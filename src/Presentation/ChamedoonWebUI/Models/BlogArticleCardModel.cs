namespace ChamedoonWebUI.Models
{
    public class BlogArticleCardModel
    {
        public string Title { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string DetailUrl { get; set; } = string.Empty;

        public string ReadMoreLabel { get; set; } = "ادامه مطلب";

        public BlogArticleMetaModel Meta { get; set; } = new BlogArticleMetaModel();
    }
}
