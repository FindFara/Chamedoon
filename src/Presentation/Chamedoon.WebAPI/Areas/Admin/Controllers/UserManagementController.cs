using Chamedoon.Application.Services.Admin.UserManagement.Command;
using Chamedoon.Application.Services.Admin.UserManagement.Query;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        private readonly IMediator mediator;

        public UserManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminUserManagement_VM adminUser, int pageSize, int pageNumber)
        {
            return View((await mediator.Send(new GetAllUsersWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                AdminPanelUser = adminUser
            })).Result);
        }
        public async Task<IActionResult> CreateUser()
        {
            return View("CreateOrEdit", new AdminCreateOrEditUser_VM());
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AdminCreateOrEditUser_VM user)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new CreateUserInAdminCommand { User = user });
                return RedirectToAction(nameof(Index));
            }
            return View("CreateOrEdit", user);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await mediator.Send(new GetAdminPanelUserDetailleQuery { UserId = id.Value });
            if (user == null)
            {
                return NotFound();
            }
            return View("CreateOrEdit", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, AdminCreateOrEditUser_VM user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await mediator.Send(new EditUserAdminCommand
                    {
                        User = new AdminCreateOrEditUser_VM
                        {
                            Id = id,
                            Email = user.Email,
                            LockoutEnabled = user.LockoutEnabled,
                            Password = user.Password,
                            UserName = user.UserName,
                        }
                    });
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View("CreateOrEdit", user);
        }


    }
}
