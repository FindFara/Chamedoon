using Chamedoon.Application.Services.Account.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Admin.UserManagement.Command;

namespace ChamedoonWebUI.Controllers
{
    [Route("cpanel")]
    public class UserPanelController : Controller
    {
        private readonly IMediator mediator;

        public UserPanelController(IMediator mediator)
        {
            this.mediator = mediator;
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
            var user = await mediator.Send(new GetUserDetailsQuery { UserName = User.Identity.Name });
            return View(user);
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

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}
