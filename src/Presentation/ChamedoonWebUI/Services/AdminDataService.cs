using System;
using System.Collections.Generic;
using System.Linq;
using ChamedoonWebUI.Areas.Admin.Models;

namespace ChamedoonWebUI.Services;

public interface IAdminDataService
{
    IReadOnlyList<AdminUser> GetUsers();
    AdminUser? GetUser(Guid id);
    AdminUser CreateUser(AdminUser user);
    bool UpdateUser(AdminUser user);
    bool DeleteUser(Guid id);

    IReadOnlyList<BlogPost> GetBlogPosts();
    BlogPost? GetBlogPost(Guid id);
    BlogPost CreateBlogPost(BlogPost post);
    bool UpdateBlogPost(BlogPost post);
    bool DeleteBlogPost(Guid id);

    IReadOnlyList<RoleDefinition> GetRoles();
    RoleDefinition? GetRole(Guid id);
    RoleDefinition CreateRole(RoleDefinition role);
    bool UpdateRole(RoleDefinition role);
    bool DeleteRole(Guid id);

    IReadOnlyList<PermissionDefinition> GetPermissions();

    DashboardSummary GetDashboardSummary();
}

public class AdminDataService : IAdminDataService
{
    private readonly object _syncRoot = new();
    private readonly List<AdminUser> _users;
    private readonly List<BlogPost> _posts;
    private readonly List<RoleDefinition> _roles;
    private readonly List<PermissionDefinition> _permissions;

    public AdminDataService()
    {
        _permissions = SeedPermissions();
        _roles = SeedRoles(_permissions);
        _users = SeedUsers(_roles);
        _posts = SeedPosts();
    }

    public AdminUser CreateUser(AdminUser user)
    {
        lock (_syncRoot)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = user.CreatedAt == default ? DateTime.UtcNow : user.CreatedAt;
            _users.Add(Clone(user));
            return Clone(user);
        }
    }

    public bool DeleteUser(Guid id)
    {
        lock (_syncRoot)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return false;
            }

            _users.Remove(existing);
            return true;
        }
    }

    public AdminUser? GetUser(Guid id)
    {
        lock (_syncRoot)
        {
            return _users.FirstOrDefault(u => u.Id == id) is { } user ? Clone(user) : null;
        }
    }

    public IReadOnlyList<AdminUser> GetUsers()
    {
        lock (_syncRoot)
        {
            return _users.Select(Clone).ToList();
        }
    }

    public bool UpdateUser(AdminUser user)
    {
        lock (_syncRoot)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index < 0)
            {
                return false;
            }

            var existing = _users[index];
            user.CreatedAt = existing.CreatedAt;
            _users[index] = Clone(user);
            return true;
        }
    }

    public BlogPost CreateBlogPost(BlogPost post)
    {
        lock (_syncRoot)
        {
            post.Id = Guid.NewGuid();
            if (post.PublishedOn == default)
            {
                post.PublishedOn = DateTime.UtcNow;
            }

            _posts.Add(Clone(post));
            return Clone(post);
        }
    }

    public bool DeleteBlogPost(Guid id)
    {
        lock (_syncRoot)
        {
            var existing = _posts.FirstOrDefault(p => p.Id == id);
            if (existing == null)
            {
                return false;
            }

            _posts.Remove(existing);
            return true;
        }
    }

    public BlogPost? GetBlogPost(Guid id)
    {
        lock (_syncRoot)
        {
            return _posts.FirstOrDefault(p => p.Id == id) is { } post ? Clone(post) : null;
        }
    }

    public IReadOnlyList<BlogPost> GetBlogPosts()
    {
        lock (_syncRoot)
        {
            return _posts.Select(Clone).ToList();
        }
    }

    public bool UpdateBlogPost(BlogPost post)
    {
        lock (_syncRoot)
        {
            var index = _posts.FindIndex(p => p.Id == post.Id);
            if (index < 0)
            {
                return false;
            }

            var existing = _posts[index];
            post.PublishedOn = post.IsPublished ? (post.PublishedOn == default ? DateTime.UtcNow : post.PublishedOn) : existing.PublishedOn;
            _posts[index] = Clone(post);
            return true;
        }
    }

    public RoleDefinition CreateRole(RoleDefinition role)
    {
        lock (_syncRoot)
        {
            role.Id = Guid.NewGuid();
            role.PermissionIds = role.PermissionIds?.Distinct().ToList() ?? new List<Guid>();
            _roles.Add(Clone(role));
            return Clone(role);
        }
    }

    public bool DeleteRole(Guid id)
    {
        lock (_syncRoot)
        {
            if (_roles.Count(r => r.Id == id) == 0)
            {
                return false;
            }

            _roles.RemoveAll(r => r.Id == id);
            if (_roles.Any())
            {
                var fallbackRoleId = _roles.First().Id;
                foreach (var user in _users.Where(u => u.RoleId == id))
                {
                    user.RoleId = fallbackRoleId;
                }
            }
            else
            {
                foreach (var user in _users.Where(u => u.RoleId == id))
                {
                    user.RoleId = Guid.Empty;
                }
            }

            return true;
        }
    }

    public RoleDefinition? GetRole(Guid id)
    {
        lock (_syncRoot)
        {
            return _roles.FirstOrDefault(r => r.Id == id) is { } role ? Clone(role) : null;
        }
    }

    public IReadOnlyList<RoleDefinition> GetRoles()
    {
        lock (_syncRoot)
        {
            return _roles.Select(Clone).ToList();
        }
    }

    public bool UpdateRole(RoleDefinition role)
    {
        lock (_syncRoot)
        {
            var index = _roles.FindIndex(r => r.Id == role.Id);
            if (index < 0)
            {
                return false;
            }

            role.PermissionIds = role.PermissionIds?.Distinct().ToList() ?? new List<Guid>();
            _roles[index] = Clone(role);
            return true;
        }
    }

    public IReadOnlyList<PermissionDefinition> GetPermissions()
    {
        lock (_syncRoot)
        {
            return _permissions.Select(Clone).ToList();
        }
    }

    public DashboardSummary GetDashboardSummary()
    {
        lock (_syncRoot)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var monthly = _users
                .GroupBy(u => new DateTime(u.CreatedAt.Year, u.CreatedAt.Month, 1))
                .OrderBy(g => g.Key)
                .TakeLast(6)
                .Select(g => new DashboardSummary.MonthlyRegistration(g.Key.ToString("yyyy/MM"), g.Count()))
                .ToList();

            var popularPosts = _posts
                .OrderByDescending(p => p.Views)
                .Take(5)
                .Select(p => new DashboardSummary.PopularPost(p.Title, p.Views, p.IsPublished))
                .ToList();

            var roleDistribution = _roles
                .Select(role => new DashboardSummary.RoleBreakdown(role.Name, _users.Count(u => u.RoleId == role.Id)))
                .ToList();

            var permissionUsage = _permissions
                .Select(permission => new DashboardSummary.PermissionUsage(
                    permission.Name,
                    _roles.Count(role => role.PermissionIds.Contains(permission.Id))))
                .ToList();

            return new DashboardSummary
            {
                TotalUsers = _users.Count,
                ActiveUsers = _users.Count(u => u.IsActive),
                NewUsersThisMonth = _users.Count(u => u.CreatedAt >= startOfMonth),
                TotalBlogPosts = _posts.Count,
                PublishedBlogPosts = _posts.Count(p => p.IsPublished),
                DraftBlogPosts = _posts.Count(p => !p.IsPublished),
                TotalViews = _posts.Sum(p => p.Views),
                PopularPosts = popularPosts,
                RoleDistribution = roleDistribution,
                PermissionUsage = permissionUsage,
                MonthlyRegistrations = monthly,
                RecentUsers = _users
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .Select(Clone)
                    .ToList(),
                RecentPosts = _posts
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(5)
                    .Select(Clone)
                    .ToList()
            };
        }
    }

    private static AdminUser Clone(AdminUser user)
    {
        return new AdminUser
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };
    }

    private static BlogPost Clone(BlogPost post)
    {
        return new BlogPost
        {
            Id = post.Id,
            Title = post.Title,
            Category = post.Category,
            IsPublished = post.IsPublished,
            PublishedOn = post.PublishedOn,
            Views = post.Views,
            Summary = post.Summary
        };
    }

    private static RoleDefinition Clone(RoleDefinition role)
    {
        return new RoleDefinition
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            PermissionIds = role.PermissionIds?.ToList() ?? new List<Guid>()
        };
    }

    private static PermissionDefinition Clone(PermissionDefinition permission)
    {
        return new PermissionDefinition
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description
        };
    }

    private static List<PermissionDefinition> SeedPermissions()
    {
        return new List<PermissionDefinition>
        {
            new() { Id = Guid.NewGuid(), Name = "مدیریت کاربران", Description = "دسترسی کامل به مدیریت کاربران" },
            new() { Id = Guid.NewGuid(), Name = "مدیریت محتوا", Description = "ایجاد، ویرایش و حذف مطالب وبلاگ" },
            new() { Id = Guid.NewGuid(), Name = "مشاهده گزارشات", Description = "دسترسی به داشبورد و گزارشات" },
            new() { Id = Guid.NewGuid(), Name = "مدیریت نقش‌ها", Description = "تعریف نقش‌ها و تخصیص مجوزها" }
        };
    }

    private static List<RoleDefinition> SeedRoles(IEnumerable<PermissionDefinition> permissions)
    {
        var permissionList = permissions.ToList();
        return new List<RoleDefinition>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "مدیر کل",
                Description = "دسترسی کامل به تمامی بخش‌های پنل",
                PermissionIds = permissionList.Select(p => p.Id).ToList()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "مدیر محتوا",
                Description = "تمرکز بر مدیریت بلاگ و محتوا",
                PermissionIds = permissionList
                    .Where(p => p.Name is "مدیریت محتوا" or "مشاهده گزارشات")
                    .Select(p => p.Id)
                    .ToList()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "پشتیبان",
                Description = "پاسخ به درخواست‌های کاربران و مشاهده گزارشات",
                PermissionIds = permissionList
                    .Where(p => p.Name is "مدیریت کاربران" or "مشاهده گزارشات")
                    .Select(p => p.Id)
                    .ToList()
            }
        };
    }

    private static List<AdminUser> SeedUsers(IEnumerable<RoleDefinition> roles)
    {
        var roleList = roles.ToList();
        return new List<AdminUser>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "سمانه محمودی",
                Email = "samaneh@example.com",
                RoleId = roleList[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "امیررضا سلطانی",
                Email = "amir@example.com",
                RoleId = roleList[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-32),
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "گلاره سادات",
                Email = "golareh@example.com",
                RoleId = roleList[2].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "سعید موسوی",
                Email = "saeed@example.com",
                RoleId = roleList[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-64),
                IsActive = false
            }
        };
    }

    private static List<BlogPost> SeedPosts()
    {
        return new List<BlogPost>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "راهنمای کامل مهاجرت به کانادا",
                Category = "راهنماها",
                IsPublished = true,
                PublishedOn = DateTime.UtcNow.AddDays(-20),
                Views = 2450,
                Summary = "از انتخاب استان تا آماده‌سازی مدارک در این راهنمای جامع توضیح داده شده است."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "۵ اشتباه رایج در درخواست ویزای دانشجویی",
                Category = "نکات کاربردی",
                IsPublished = true,
                PublishedOn = DateTime.UtcNow.AddDays(-10),
                Views = 1680,
                Summary = "پیش از ارسال درخواست ویزا این پنج نکته مهم را مرور کنید."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "مصاحبه با مهاجر موفق",
                Category = "تجربه‌ها",
                IsPublished = false,
                PublishedOn = DateTime.UtcNow.AddDays(-2),
                Views = 220,
                Summary = "نسخه پیش‌نویس برای بررسی نهایی تیم محتوا."
            }
        };
    }
}
