using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.Command;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Register.Command
{
    public class RegisterUserWithGoogleCommand : IRequest<OperationResult>
    {
        public ExternalLoginInfo ExternalLoginInfo { get; set; }
        public string Email { get; set; }
    }
    public class RegisterUserWithGoogleCommandHandler : IRequestHandler<RegisterUserWithGoogleCommand, OperationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public RegisterUserWithGoogleCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }
        public async Task<OperationResult> Handle(RegisterUserWithGoogleCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist is not null)
                return OperationResult.Fail();

            User user = new User
            {
                UserName = string.Concat("U-", StringExtensions.GenerateRandomString(8)),
                Email = request.Email
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return OperationResult.Fail(createResult.Errors.Select(e => e.Description).First());

            var addCustomer = await _mediator.Send(new AddCustomerCommand { Id = user.Id });
            if (addCustomer.IsSuccess is false)
                return OperationResult.Fail(addCustomer.Message);

            await _userManager.AddLoginAsync(user, request.ExternalLoginInfo);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return OperationResult.Success();
        }
    }
}
