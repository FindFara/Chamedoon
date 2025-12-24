using Chamedoon.Application.Services.Admin.Subscriptions;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class SubscriptionPlansController : Controller
{
    private readonly IAdminSubscriptionPlanService _planService;

    public SubscriptionPlansController(IAdminSubscriptionPlanService planService)
    {
        _planService = planService;
    }

    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _planService.GetPlansAsync(search, page, pageSize, cancellationToken);
        if (!result.IsSuccess || result.Result is null)
        {
            return Problem(result.Message);
        }

        var model = new SubscriptionPlanIndexViewModel
        {
            Plans = result.Result.Items.Select(SubscriptionPlanListItemViewModel.FromDto).ToList(),
            SearchTerm = search,
            CurrentPage = result.Result.PageNumber,
            TotalPages = result.Result.TotalPages,
            PageSize = pageSize
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View("Edit", new SubscriptionPlanEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionPlanEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var result = await _planService.CreatePlanAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View("Edit", model);
        }

        TempData["Success"] = "پلن جدید ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
    {
        var result = await _planService.GetPlanAsync(id, cancellationToken);
        if (!result.IsSuccess || result.Result is null)
        {
            return NotFound();
        }

        return View(SubscriptionPlanEditViewModel.FromDto(result.Result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, SubscriptionPlanEditViewModel model, CancellationToken cancellationToken)
    {
        if (!string.Equals(id, model.Id, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _planService.UpdatePlanAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = "پلن به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _planService.DeactivatePlanAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(result.Message);
        }

        TempData["Success"] = "پلن غیرفعال شد.";
        return RedirectToAction(nameof(Index));
    }
}
