using AutoMapper;
using Chamedoon.Application.Services.Admin.UserManagement.Command;
using Chamedoon.Application.Services.Admin.UserManagement.Query;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Application.Services.Blog.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public UserManagementController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await mediator.Send(new GetAdminPanelUserDetailleQuery { UserId = id });

            if (user.IsSuccess == false)
            {
                return NotFound();
            }

            return View(user.Result);
        }

        public async Task<IActionResult> CreateUser()
        {
            return View("CreateOrEdit", new AdminCreateOrEditUser_VM());
        }
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
        public async Task<IActionResult> EditUser(long id)
        {
            if (id == 0)
                return NotFound();

            var user = await mediator.Send(new GetAdminPanelUserDetailleQuery { UserId = id });
            if (user.IsSuccess is false)
                return NotFound();

            var editUser = mapper.Map<AdminCreateOrEditUser_VM>(user.Result);
            if (editUser == null)
                return NotFound();

            return View("CreateOrEdit", editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, AdminCreateOrEditUser_VM user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            await mediator.Send(new EditUserAdminCommand { User = user });

            return RedirectToAction(nameof(Index));
        }
    }
}
