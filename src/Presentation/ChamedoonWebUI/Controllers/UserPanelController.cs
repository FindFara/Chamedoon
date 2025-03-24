using Chamedoon.Application.Services.Account.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Admin.UserManagement.Command;
using Chamedoon.Application.Services.Customers.Query;
using Chamedoon.Application.Services.Customers.Command;
using AutoMapper;
using Chamedoon.Application.Services.Customers.ViewModel;

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

        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var Customer = await mediator.Send(new GetUserAndCustomerDetailsQuery { UserName = User.Identity.Name });

            return View(Customer.Result);
        }

        [Route("EditProfile")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUser_VM user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var edit = await mediator.Send(new EditUserCommand { User = user }); 
            if (edit.IsSuccess is false)
                return View(user);

            var editCustomer = await mediator.Send(new UpsertCustomerCommand { 
                UpsertCustomerViewModel = mapper.Map<UpsertCustomerViewModel>(user)
            });

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}
