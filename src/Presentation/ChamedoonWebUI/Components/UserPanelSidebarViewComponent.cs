using Chamedoon.Application.Services.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Components
{
    public class UserPanelSidebarViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public UserPanelSidebarViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            var userId = User?.Identity?.Name;
            var customer =await mediator.Send(new GetCustomerProfileQuery { CustomerId = long.Parse(userId) });
            return View("Profile", customer.Result);
        }
    }
}
