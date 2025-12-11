using Chamedoon.Application.Services.Admin.Blogs;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogsController : Controller
{
    private readonly IAdminBlogService _blogService;

    public BlogsController(IAdminBlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        var postsResult = await _blogService.GetPostsAsync(search, cancellationToken);
        if (!postsResult.IsSuccess || postsResult.Result is null)
        {
            return Problem(postsResult.Message);
        }

        var model = new BlogsIndexViewModel
        {
            Posts = postsResult.Result.Select(BlogListItemViewModel.FromDto).ToList(),
            SearchTerm = search
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View("Edit", new BlogEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var result = await _blogService.CreatePostAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View("Edit", model);
        }

        TempData["Success"] = "مقاله جدید با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        var postResult = await _blogService.GetPostAsync(id, cancellationToken);
        if (!postResult.IsSuccess || postResult.Result is null)
        {
            return NotFound();
        }

        var model = BlogEditViewModel.FromDto(postResult.Result);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, BlogEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _blogService.UpdatePostAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = "مقاله با موفقیت به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await _blogService.DeletePostAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(result.Message);
        }

        TempData["Success"] = "مقاله حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
