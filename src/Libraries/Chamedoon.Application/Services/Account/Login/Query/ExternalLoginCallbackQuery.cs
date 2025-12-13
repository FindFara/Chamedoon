using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Customers.Command;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Chamedoon.Application.Services.Account.Login.Query
{
    public class ExternalLoginCallbackQuery : IRequest<OperationResult<ExternalLoginViewModel>>
    {
    }

    public class ExternalLoginCallbackHandler : IRequestHandler<ExternalLoginCallbackQuery, OperationResult<ExternalLoginViewModel>>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ExternalLoginCallbackHandler(SignInManager<User> signInManager, UserManager<User> userManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<OperationResult<ExternalLoginViewModel>> Handle(ExternalLoginCallbackQuery request, CancellationToken cancellationToken)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return OperationResult<ExternalLoginViewModel>.Fail();
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return OperationResult<ExternalLoginViewModel>.Success(new ExternalLoginViewModel { IsSuccess = true });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return OperationResult<ExternalLoginViewModel>.Fail();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = await UsernameGenerator.GenerateUniqueAsync(_userManager, cancellationToken),
                    Email = email
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return OperationResult<ExternalLoginViewModel>.Fail();
                }

                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);

                var addCustomer = await _mediator.Send(new AddCustomerCommand { Id = user.Id }, cancellationToken);
                if (!addCustomer.IsSuccess)
                {
                    return OperationResult<ExternalLoginViewModel>.Fail();
                }
            }

            return OperationResult<ExternalLoginViewModel>.Success(new ExternalLoginViewModel
            {
                IsSuccess = true,
                Email = email,
                LoginProvider = info.LoginProvider
            });
        }
    }
}
