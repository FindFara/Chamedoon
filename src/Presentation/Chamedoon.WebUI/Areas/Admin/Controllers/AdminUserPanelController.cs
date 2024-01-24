using Chamedoon.Application.Services.Admin.UserManagement.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    public class AdminUserPanelController : AdminBaseController
    {
        private readonly IMediator mediator;

        public AdminUserPanelController(IMediator mediator)
        {
            this.mediator=mediator;
        }
        public async Task<IActionResult> Index(GetAllUsersWithPaginationQuery query)
        {
            return View(mediator.Send(query));
        }
    }
}
