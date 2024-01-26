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

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminUserManagement_VM adminUser,int pageSize, int pageNumber)
        {
            return View((await mediator.Send(new GetAllUsersWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                AdminPanelUser = adminUser
            })).Result);
        }




    }
}
