using Chamedoon.Application.Services.Admin.UserManagement.Query;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        private readonly IMediator mediator;

        public UserManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IActionResult> Index(int pagesize, int pageNumber, AdminUserManagement_VM adminUser)
        {
            return View((await mediator.Send(new GetAllUsersWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pagesize,
                AdminPanelUser = adminUser
            })).Result);
        }
    }
}
