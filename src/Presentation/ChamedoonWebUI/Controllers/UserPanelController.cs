using Chamedoon.Application.Services.Account.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Customers.Query;
using Chamedoon.Application.Services.Customers.Command;
using AutoMapper;
using Chamedoon.Application.Services.Customers.ViewModel;
using System.Security.Claims;

namespace ChamedoonWebUI.Controllers
{
    [Route("cpanel")]
    public class UserPanelController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public UserPanelController(IMediator mediator ,IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            var user = await mediator.Send(new GetUserDetailsQuery { UserName = User.Identity.Name });
            return View(user.Result);
        }
        #endregion

        #region EditProfile

        [Route("Edit")]
        public async Task<IActionResult> Edit()
        {
            var Customer = await mediator.Send(new GetUserAndCustomerDetailsQuery { UserName = User.Identity.Name });

            return View(Customer.Result);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUser_VM user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            user.Id = long.Parse(userId);
            var editCustomer = await mediator.Send(new UpdateCustomerCommand
            {
                UpsertCustomerViewModel = mapper.Map<UpsertCustomerViewModel>(user),
            });

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Edit", "UserPanel");
        }
        #endregion
    }
}
