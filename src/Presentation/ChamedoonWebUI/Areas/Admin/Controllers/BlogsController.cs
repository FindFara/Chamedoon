using ChamedoonWebUI.Areas.Admin.Models;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using ChamedoonWebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogsController : Controller
{
    private readonly IAdminDataService _dataService;

    public BlogsController(IAdminDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index(string? search, string? category)
    {
        var posts = _dataService.GetBlogPosts();
        var categories = posts
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c)
            .ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            posts = posts
                .Where(p => p.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            p.Summary.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            posts = posts.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var model = new BlogIndexViewModel
        {
            Posts = posts,
            SearchTerm = search,
            Category = category,
            Categories = categories
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View("Edit", new BlogEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BlogEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var post = new BlogPost
        {
            Title = model.Title,
            Category = model.Category,
            IsPublished = model.IsPublished,
            PublishedOn = model.PublishedOn ?? DateTime.UtcNow,
            Summary = model.Summary,
            Views = model.Views
        };

        _dataService.CreateBlogPost(post);
        TempData["Success"] = "مطلب جدید ثبت شد.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        var post = _dataService.GetBlogPost(id);
        if (post == null)
        {
            return NotFound();
        }

        var model = new BlogEditViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Category = post.Category,
            IsPublished = post.IsPublished,
            PublishedOn = post.PublishedOn,
            Summary = post.Summary,
            Views = post.Views
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, BlogEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existing = _dataService.GetBlogPost(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Title = model.Title;
        existing.Category = model.Category;
        existing.IsPublished = model.IsPublished;
        existing.PublishedOn = model.PublishedOn ?? existing.PublishedOn;
        existing.Summary = model.Summary;
        existing.Views = model.Views;

        _dataService.UpdateBlogPost(existing);
        TempData["Success"] = "مطلب به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        if (!_dataService.DeleteBlogPost(id))
        {
            return NotFound();
        }

        TempData["Success"] = "مطلب حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
