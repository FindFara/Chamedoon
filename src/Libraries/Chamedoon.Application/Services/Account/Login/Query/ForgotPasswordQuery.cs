using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Query
{
    public class ForgotPasswordQuery : IRequest<bool>
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordQuery, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;

        public ForgotPasswordHandler(UserManager<User> userManager, IEmailService emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<bool> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://yoursite.com/Account/ResetPassword?email={request.Email}&token={token}";

            await _emailSender.SendMail(request.Email, "بازیابی رمز عبور", $"<a href='{resetLink}'>اینجا کلیک کنید</a>");

            return true;
        }
    }
}
