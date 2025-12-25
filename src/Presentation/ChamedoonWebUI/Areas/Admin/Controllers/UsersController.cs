using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Admin.Users;
using Chamedoon.Application.Services.Subscription;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IAdminUserService _userService;
    private readonly IMediator _mediator;

    public UsersController(IAdminUserService userService, IMediator mediator)
    {
        _userService = userService;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(
        string? search,
        long? roleId,
        string? subscriptionPlanId,
        DateTime? fromDate,
        DateTime? toDate,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var allowedPageSizes = new[] { 10, 15, 25 };
        page = page < 1 ? 1 : page;
        pageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;

        var usersResult = await _userService.GetUsersAsync(search, roleId, subscriptionPlanId, fromDate, toDate, page, pageSize, cancellationToken);
        var rolesResult = await _userService.GetRolesAsync(cancellationToken);
        var plansResult = await _mediator.Send(new GetSubscriptionPlansQuery(true), cancellationToken);
        if (!usersResult.IsSuccess || usersResult.Result is null)
        {
            return Problem(usersResult.Message);
        }

        if (!rolesResult.IsSuccess || rolesResult.Result is null)
        {
            return Problem(rolesResult.Message);
        }

        var model = new UsersIndexViewModel
        {
            Users = usersResult.Result.Items.Select(UserListItemViewModel.FromDto).ToList(),
            Roles = rolesResult.Result.Select(role => new RoleOptionViewModel { Id = role.Id, Name = role.Name }).ToList(),
            Plans = plansResult.Select(plan => new SubscriptionPlanOptionViewModel { Id = plan.Id, Title = plan.Title }).ToList(),
            SearchTerm = search,
            SelectedRoleId = roleId,
            SelectedSubscriptionPlanId = subscriptionPlanId,
            FromDate = fromDate,
            ToDate = toDate,
            CurrentPage = usersResult.Result.PageNumber,
            TotalPages = usersResult.Result.TotalPages,
            PageSize = pageSize,
            TotalCount = usersResult.Result.TotalCount
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var roles = await _userService.GetRolesAsync(cancellationToken);
        if (!roles.IsSuccess || roles.Result is null)
        {
            return Problem(roles.Message);
        }

        var model = new UserEditViewModel
        {
            Roles = roles.Result.Select(role => new RoleOptionViewModel { Id = role.Id, Name = role.Name }).ToList(),
            IsActive = true
        };

        await PopulatePlansAsync(model, cancellationToken);

        return View("Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRolesAsync(model, cancellationToken);
            await PopulatePlansAsync(model, cancellationToken);
            return View("Edit", model);
        }

        var input = model.ToInput();
        var result = await _userService.CreateUserAsync(input, cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await PopulateRolesAsync(model, cancellationToken);
            return View("Edit", model);
        }

        TempData["Success"] = "کاربر جدید با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        var userResult = await _userService.GetUserAsync(id, cancellationToken);
        var rolesResult = await _userService.GetRolesAsync(cancellationToken);
        if (!userResult.IsSuccess || userResult.Result is null)
        {
            return NotFound();
        }

        if (!rolesResult.IsSuccess || rolesResult.Result is null)
        {
            return Problem(rolesResult.Message);
        }

        var model = UserEditViewModel.FromDto(userResult.Result);
        model.Roles = rolesResult.Result.Select(role => new RoleOptionViewModel { Id = role.Id, Name = role.Name }).ToList();
        await PopulatePlansAsync(model, cancellationToken);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, UserEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulateRolesAsync(model, cancellationToken);
            await PopulatePlansAsync(model, cancellationToken);
            return View(model);
        }

        var result = await _userService.UpdateUserAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await PopulateRolesAsync(model, cancellationToken);
            await PopulatePlansAsync(model, cancellationToken);
            return View(model);
        }

        TempData["Success"] = "اطلاعات کاربر با موفقیت به‌روزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteUserAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Problem(result.Message);
        }

        TempData["Success"] = "کاربر حذف شد.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateRolesAsync(UserEditViewModel model, CancellationToken cancellationToken)
    {
        var roles = await _userService.GetRolesAsync(cancellationToken);
        model.Roles = roles.IsSuccess && roles.Result is not null
            ? roles.Result.Select(role => new RoleOptionViewModel { Id = role.Id, Name = role.Name }).ToList()
            : new List<RoleOptionViewModel>();
    }

    private async Task PopulatePlansAsync(UserEditViewModel model, CancellationToken cancellationToken)
    {
        var plans = await _mediator.Send(new GetSubscriptionPlansQuery(true), cancellationToken);
        model.Plans = plans.Select(plan => new SubscriptionPlanOptionViewModel { Id = plan.Id, Title = plan.Title }).ToList();
    }
}
