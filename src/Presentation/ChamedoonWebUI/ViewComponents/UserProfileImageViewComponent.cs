using Chamedoon.Application.Services.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Components
{
    public class UserProfileImageViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public UserProfileImageViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var username = User?.Identity?.Name;
            var customer =await mediator.Send(new GetCustomerProfileQuery { UserName = username });
            return View("Profile", customer.Result);
        }
    }
}
