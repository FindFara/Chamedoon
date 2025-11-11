using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Admin.Roles;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class RolesController : Controller
{
    private readonly IAdminRoleService _roleService;

    public RolesController(IAdminRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var rolesResult = await _roleService.GetRolesAsync(cancellationToken);
        if (!rolesResult.IsSuccess || rolesResult.Result is null)
        {
            return Problem(rolesResult.Message);
        }

        var model = new RolesIndexViewModel
        {
            Roles = rolesResult.Result.Select(RoleListItemViewModel.FromDto).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var permissions = await _roleService.GetPermissionNamesAsync(cancellationToken);
        if (!permissions.IsSuccess || permissions.Result is null)
        {
            return Problem(permissions.Message);
        }

        var model = new RoleEditViewModel
        {
            AvailablePermissions = permissions.Result
        };

        return View("Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulatePermissionsAsync(model, cancellationToken);
            return View("Edit", model);
        }

        var input = model.ToInput();
        var result = await _roleService.CreateRoleAsync(input, cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await PopulatePermissionsAsync(model, cancellationToken);
            return View("Edit", model);
        }

        TempData["Success"] = "نقش جدید با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        var roleResult = await _roleService.GetRoleAsync(id, cancellationToken);
        var permissionsResult = await _roleService.GetPermissionNamesAsync(cancellationToken);
        if (!roleResult.IsSuccess || roleResult.Result is null)
        {
            return NotFound();
        }

        if (!permissionsResult.IsSuccess || permissionsResult.Result is null)
        {
            return Problem(permissionsResult.Message);
        }

        var model = RoleEditViewModel.FromDto(roleResult.Result);
        model.AvailablePermissions = permissionsResult.Result;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, RoleEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulatePermissionsAsync(model, cancellationToken);
            return View(model);
        }

        var result = await _roleService.UpdateRoleAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await PopulatePermissionsAsync(model, cancellationToken);
            return View(model);
        }

        TempData["Success"] = "اطلاعات نقش با موفقیت به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(result.Message);
        }

        TempData["Success"] = "نقش حذف شد.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulatePermissionsAsync(RoleEditViewModel model, CancellationToken cancellationToken)
    {
        var permissions = await _roleService.GetPermissionNamesAsync(cancellationToken);
        model.AvailablePermissions = permissions.IsSuccess && permissions.Result is not null
            ? permissions.Result
            : Array.Empty<string>();
    }
}
