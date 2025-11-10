using ChamedoonWebUI.Areas.Admin.Models;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using ChamedoonWebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class RolesController : Controller
{
    private readonly IAdminDataService _dataService;

    public RolesController(IAdminDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index()
    {
        var model = new RolesIndexViewModel
        {
            Roles = _dataService.GetRoles(),
            Permissions = _dataService.GetPermissions()
        };

        return View(model);
    }

    public IActionResult Create()
    {
        var model = new RoleEditViewModel
        {
            Permissions = _dataService.GetPermissions()
        };

        return View("Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RoleEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Permissions = _dataService.GetPermissions();
            return View("Edit", model);
        }

        var role = new RoleDefinition
        {
            Name = model.Name,
            Description = model.Description,
            PermissionIds = model.SelectedPermissions
        };

        _dataService.CreateRole(role);
        TempData["Success"] = "نقش جدید ثبت شد.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        var role = _dataService.GetRole(id);
        if (role == null)
        {
            return NotFound();
        }

        var model = new RoleEditViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            SelectedPermissions = role.PermissionIds.ToList(),
            Permissions = _dataService.GetPermissions()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, RoleEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.Permissions = _dataService.GetPermissions();
            return View(model);
        }

        var existing = _dataService.GetRole(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = model.Name;
        existing.Description = model.Description;
        existing.PermissionIds = model.SelectedPermissions;

        _dataService.UpdateRole(existing);
        TempData["Success"] = "نقش به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        if (!_dataService.DeleteRole(id))
        {
            return NotFound();
        }

        TempData["Success"] = "نقش حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
