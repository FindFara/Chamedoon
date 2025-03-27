using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Command
{
    public class SignInUserCommand : IRequest<OperationResult>
    {
        public string UserId { get; set; }
        public bool IsPersistent { get; set; }
    }
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, OperationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SignInUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<OperationResult> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return OperationResult.Fail();
            }

            await _signInManager.SignInAsync(user, isPersistent: request.IsPersistent);
            return OperationResult.Success();
        }
    }
}
