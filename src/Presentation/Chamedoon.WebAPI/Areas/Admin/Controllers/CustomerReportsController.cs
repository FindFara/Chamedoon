using Chamedoon.Application.Services.Admin.CustomerReports.Query;
using Chamedoon.WebUI.Areas.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerReportsController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public CustomerReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetCustomerReportsQuery());
            if (!result.IsSuccess || result.Result == null)
            {
                return View("~/Areas/Admin/Views/Shared/Error.cshtml");
            }

            return View("~/Areas/Admin/Views/CustomerReports/Index.cshtml", result.Result);
        }
    }
}
