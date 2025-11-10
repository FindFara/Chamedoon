using System.ComponentModel.DataAnnotations;
using ChamedoonWebUI.Areas.Admin.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class BlogIndexViewModel
{
    public IEnumerable<BlogPost> Posts { get; set; } = Enumerable.Empty<BlogPost>();
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
}

public class BlogEditViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "عنوان را وارد کنید.")]
    [Display(Name = "عنوان")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "دسته‌بندی را وارد کنید.")]
    [Display(Name = "دسته‌بندی")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "منتشر شود؟")]
    public bool IsPublished { get; set; } = true;

    [Display(Name = "تاریخ انتشار")]
    [DataType(DataType.Date)]
    public DateTime? PublishedOn { get; set; }

    [Display(Name = "خلاصه")]
    [DataType(DataType.MultilineText)]
    public string Summary { get; set; } = string.Empty;

    [Display(Name = "بازدید")]
    public int Views { get; set; }
}
