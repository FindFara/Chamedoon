using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class BlogsIndexViewModel
{
    public IReadOnlyList<BlogListItemViewModel> Posts { get; init; } = Array.Empty<BlogListItemViewModel>();
    public string? SearchTerm { get; init; }
}

public class BlogListItemViewModel
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Writer { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public long VisitCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }

    public static BlogListItemViewModel FromDto(AdminBlogPostDto dto)
        => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Writer = dto.Writer,
            ShortDescription = dto.ShortDescription,
            VisitCount = dto.VisitCount,
            CreatedAt = dto.CreatedAt,
            LastModifiedAt = dto.LastModifiedAt
        };
}

public class BlogEditViewModel
{
    public long? Id { get; set; }

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "نویسنده")]
    [Required(ErrorMessage = "وارد کردن نام نویسنده الزامی است.")]
    public string Writer { get; set; } = string.Empty;

    [Display(Name = "خلاصه")]
    [Required(ErrorMessage = "وارد کردن خلاصه الزامی است.")]
    [MaxLength(500, ErrorMessage = "حداکثر ۵۰۰ کاراکتر مجاز است.")]
    public string ShortDescription { get; set; } = string.Empty;

    [Display(Name = "متن مقاله")]
    [Required(ErrorMessage = "وارد کردن متن مقاله الزامی است.")]
    public string ArticleDescription { get; set; } = string.Empty;

    [Display(Name = "تصویر")]
    public string? ImageName { get; set; }

    [Display(Name = "تعداد بازدید")]
    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نامعتبر است.")]
    public long VisitCount { get; set; }

    public AdminBlogPostInput ToInput()
        => new()
        {
            Id = Id,
            Title = Title,
            Writer = Writer,
            ShortDescription = ShortDescription,
            ArticleDescription = ArticleDescription,
            ImageName = ImageName,
            VisitCount = VisitCount
        };

    public static BlogEditViewModel FromDto(AdminBlogPostDto dto)
        => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Writer = dto.Writer,
            ShortDescription = dto.ShortDescription,
            ArticleDescription = dto.ArticleDescription,
            ImageName = dto.ImageName,
            VisitCount = dto.VisitCount
        };
}
