using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Command;

public class ExternalLoginCommand : IRequest<OperationResult<AuthenticationProperties>>
{
    public string Provider { get; set; }
    public string RedirectUrl { get; set; }
}
public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, OperationResult<AuthenticationProperties>>
{
    private readonly SignInManager<User> _signInManager;

    public ExternalLoginHandler(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<OperationResult<AuthenticationProperties>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, request.RedirectUrl);
        if (properties is null)
            return OperationResult<AuthenticationProperties>.Fail();

        return OperationResult<AuthenticationProperties>.Success(properties);
    }
}
