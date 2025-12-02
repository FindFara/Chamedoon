using Chamedoon.Application.Services.Customers;
using Chamedoon.Application.Services.Customers.Query;
using Chamedoon.Application.Services.Customers.ViewModel;
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

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Profile")
        {
            var username = User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
            {
                return View<CustomerProfileViewModel>(viewName, BuildDefaultProfile());
            }

            var customer = await mediator.Send(new GetCustomerProfileQuery { UserName = username });
            if (!customer.IsSuccess || customer.Result is null)
            {
                return View<CustomerProfileViewModel>(viewName, BuildDefaultProfile());
            }

            customer.Result.ProfileImage = ProfileImageHelper.NormalizeProfileImage(customer.Result.ProfileImage);

            return View<CustomerProfileViewModel>(viewName, customer.Result);
        }

        private static CustomerProfileViewModel BuildDefaultProfile() => new()
        {
            ProfileImage = ProfileImageHelper.GetDefaultBase64(),
            FirstName = "مهمان",
            LastName = string.Empty,
        };
    }
}
