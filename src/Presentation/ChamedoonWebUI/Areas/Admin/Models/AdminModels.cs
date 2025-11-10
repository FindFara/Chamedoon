using System.ComponentModel.DataAnnotations;

namespace ChamedoonWebUI.Areas.Admin.Models;

public class AdminUser
{
    public Guid Id { get; set; }

    [Display(Name = "نام کامل")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "نقش")]
    public Guid RoleId { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "فعال است؟")]
    public bool IsActive { get; set; }
}

public class BlogPost
{
    public Guid Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "دسته‌بندی")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "منتشر شده؟")]
    public bool IsPublished { get; set; }

    [Display(Name = "تاریخ انتشار")]
    public DateTime PublishedOn { get; set; }

    [Display(Name = "بازدید")]
    public int Views { get; set; }

    [Display(Name = "خلاصه")]
    public string Summary { get; set; } = string.Empty;
}

public class RoleDefinition
{
    public Guid Id { get; set; }

    [Display(Name = "نام نقش")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "توضیحات")]
    public string Description { get; set; } = string.Empty;

    public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
}

public class PermissionDefinition
{
    public Guid Id { get; set; }

    [Display(Name = "مجوز")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "توضیحات")]
    public string Description { get; set; } = string.Empty;
}

public class DashboardSummary
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int TotalBlogPosts { get; set; }
    public int PublishedBlogPosts { get; set; }
    public int DraftBlogPosts { get; set; }
    public int TotalViews { get; set; }
    public IReadOnlyList<PopularPost> PopularPosts { get; set; } = Array.Empty<PopularPost>();
    public IReadOnlyList<RoleBreakdown> RoleDistribution { get; set; } = Array.Empty<RoleBreakdown>();
    public IReadOnlyList<PermissionUsage> PermissionUsage { get; set; } = Array.Empty<PermissionUsage>();
    public IReadOnlyList<MonthlyRegistration> MonthlyRegistrations { get; set; } = Array.Empty<MonthlyRegistration>();
    public IReadOnlyList<AdminUser> RecentUsers { get; set; } = Array.Empty<AdminUser>();
    public IReadOnlyList<BlogPost> RecentPosts { get; set; } = Array.Empty<BlogPost>();

}
public record PopularPost(string Title, int Views, bool IsPublished);

public record RoleBreakdown(string RoleName, int UserCount);

public record PermissionUsage(string PermissionName, int RoleCount);

public record MonthlyRegistration(string Month, int Count);