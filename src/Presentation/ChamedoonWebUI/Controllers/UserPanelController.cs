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
using Chamedoon.Application.Services.Subscription;
using System.Security.Claims;

namespace ChamedoonWebUI.Controllers
{
    [Route("cpanel")]
    public class UserPanelController : Controller
    {
        private const long MaxProfileImageSizeBytes = 1024 * 1024;
        private const string ProfileImageSizeErrorMessage = "حجم عکس پروفایل نباید بیشتر از ۱ مگابایت باشد.";

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
            ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));
            return View(user.Result);
        }
        #endregion

        #region EditProfile

        [Route("Edit")]
        public async Task<IActionResult> Edit()
        {
            var Customer = await mediator.Send(new GetUserAndCustomerDetailsQuery { UserName = User.Identity.Name });
            ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));

            return View(Customer.Result);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUser_VM user)
        {
            if (user.ProfileImageFile?.Length > MaxProfileImageSizeBytes)
            {
                ModelState.AddModelError(nameof(user.ProfileImageFile), ProfileImageSizeErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));
                var customer = await mediator.Send(new GetUserAndCustomerDetailsQuery { UserName = User.Identity.Name });
                var viewModel = customer.Result ?? new CustomerDetailsViewModel();
                viewModel.FirstName = user.FirstName;
                viewModel.LastName = user.LastName;
                viewModel.Job = user.Job;
                viewModel.Description = user.Description;
                viewModel.Gender = user.Gender;
                return View(viewModel);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            user.Id = long.Parse(userId);
            var editCustomer = await mediator.Send(new UpdateCustomerCommand
            {
                UpsertCustomerViewModel = mapper.Map<UpsertCustomerViewModel>(user),
            });

            return RedirectToAction("Edit", "UserPanel");
        }
        #endregion
    }
}
