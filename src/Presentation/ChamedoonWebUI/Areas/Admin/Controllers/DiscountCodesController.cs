using Chamedoon.Application.Services.Admin.Discounts;
using Chamedoon.Domin.Enums;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DiscountCodesController : Controller
{
    private readonly IAdminDiscountCodeService _discountCodeService;

    public DiscountCodesController(IAdminDiscountCodeService discountCodeService)
    {
        _discountCodeService = discountCodeService;
    }

    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _discountCodeService.GetDiscountCodesAsync(search, page, pageSize, cancellationToken);
        if (!result.IsSuccess || result.Result is null)
        {
            return Problem(result.Message);
        }

        var model = new DiscountCodeIndexViewModel
        {
            Codes = result.Result.Items.Select(DiscountCodeListItemViewModel.FromDto).ToList(),
            SearchTerm = search,
            CurrentPage = result.Result.PageNumber,
            TotalPages = result.Result.TotalPages,
            PageSize = 20
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View("Edit", new DiscountCodeEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DiscountCodeEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var result = await _discountCodeService.CreateDiscountCodeAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View("Edit", model);
        }

        TempData["Success"] = "کد تخفیف جدید ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        var result = await _discountCodeService.GetDiscountCodeAsync(id, cancellationToken);
        if (!result.IsSuccess || result.Result is null)
        {
            return NotFound();
        }

        return View(DiscountCodeEditViewModel.FromDto(result.Result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, DiscountCodeEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _discountCodeService.UpdateDiscountCodeAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = "کد تخفیف به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await _discountCodeService.DeleteDiscountCodeAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(result.Message);
        }

        TempData["Success"] = "کد تخفیف حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
