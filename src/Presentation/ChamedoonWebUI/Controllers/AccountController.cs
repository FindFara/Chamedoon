using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers
{
    [Route("auth")]
    public class AccountController : Controller
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        #region Register
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser_VM register)
        {
            if (ModelState.IsValid)
            {
              var response = await mediator
                    .Send(new ManageRegisterUserCommand { RegisterUser = register });
            }
            else
            {
                return View(register);
            }

            //TODO: Activation Send Email
            //TODO: Redirect to succssesfullView

            return RedirectToAction("Login", "Account");
        }
        #endregion

    }
}
