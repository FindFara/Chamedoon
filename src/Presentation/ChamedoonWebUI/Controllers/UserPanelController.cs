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
using Chamedoon.Application.Services.Immigration;
using System.Security.Claims;
using System.Linq;

namespace ChamedoonWebUI.Controllers
{
    public record UserEvaluationResult_VM(
        long Id,
        DateTime CreatedAtUtc,
        ImmigrationInput Input,
        IReadOnlyList<CountryRecommendation> TopCountries);

    [Route("cpanel")]
    public class UserPanelController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IImmigrationEvaluationService evaluationService;

        public UserPanelController(IMediator mediator, IMapper mapper, IImmigrationEvaluationService evaluationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.evaluationService = evaluationService;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            var user = await mediator.Send(new GetUserDetailsQuery { UserName = User.Identity.Name });
            ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));
            return View(user.Result);
        }
        #endregion

        #region Evaluations

        [Route("Evaluations")]
        public async Task<IActionResult> Evaluations()
        {
            ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));
            var evaluations = await evaluationService.GetUserEvaluationsAsync(User, HttpContext.RequestAborted);
            var results = new List<UserEvaluationResult_VM>();

            foreach (var evaluation in evaluations)
            {
                var result = await mediator.Send(new ImmigrationQuery { Input = evaluation.Input });
                results.Add(new UserEvaluationResult_VM(
                    evaluation.Id,
                    evaluation.CreatedAtUtc,
                    evaluation.Input,
                    result.TopCountries?.Take(3).ToList() ?? new List<CountryRecommendation>()));
            }

            return View(results);
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
            if (!ModelState.IsValid)
            {
                ViewBag.SubscriptionStatus = await mediator.Send(new GetSubscriptionStatusQuery(User));
                return View(user);
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
