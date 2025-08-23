using Chamedoon.Application.Services.Blog.ViewModel;
namespace ChamedoonWebUI.Models
{
    public class BlogIndexViewModel
    {
        public IEnumerable<BlogViewModel> Articles { get; set; } = new List<BlogViewModel>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Search { get; set; }
        public string? Writer { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}