using ChamedoonWebUI.Areas.Admin.Models;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using ChamedoonWebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class UsersController : Controller
{
    private readonly IAdminDataService _dataService;

    public UsersController(IAdminDataService dataService)
    {
        _dataService = dataService;
    }

    public IActionResult Index(string? search, Guid? roleId)
    {
        var users = _dataService.GetUsers();
        if (!string.IsNullOrWhiteSpace(search))
        {
            users = users
                .Where(u => u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            u.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (roleId.HasValue)
        {
            users = users.Where(u => u.RoleId == roleId.Value).ToList();
        }

        var model = new UsersIndexViewModel
        {
            Users = users,
            Roles = _dataService.GetRoles(),
            SearchTerm = search,
            SelectedRoleId = roleId
        };

        return View(model);
    }

    public IActionResult Create()
    {
        var model = new UserEditViewModel
        {
            Roles = _dataService.GetRoles()
        };

        return View("Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Roles = _dataService.GetRoles();
            return View("Edit", model);
        }

        var user = new AdminUser
        {
            FullName = model.FullName,
            Email = model.Email,
            RoleId = model.RoleId,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _dataService.CreateUser(user);
        TempData["Success"] = "کاربر جدید با موفقیت اضافه شد.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        var user = _dataService.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            IsActive = user.IsActive,
            Roles = _dataService.GetRoles()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, UserEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.Roles = _dataService.GetRoles();
            return View(model);
        }

        var existing = _dataService.GetUser(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.FullName = model.FullName;
        existing.Email = model.Email;
        existing.RoleId = model.RoleId;
        existing.IsActive = model.IsActive;

        _dataService.UpdateUser(existing);
        TempData["Success"] = "اطلاعات کاربر به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        if (!_dataService.DeleteUser(id))
        {
            return NotFound();
        }

        TempData["Success"] = "کاربر حذف شد.";
        return RedirectToAction(nameof(Index));
    }
}
