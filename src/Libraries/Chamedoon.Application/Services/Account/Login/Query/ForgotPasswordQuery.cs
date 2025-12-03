using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chamedoon.Application.Services.Account.Login.Query
{
    public class ForgotPasswordQuery : IRequest<OperationResult>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string ResetLinkAction{ get; set; }
    }
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordQuery, OperationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;

        public ForgotPasswordHandler(UserManager<User> userManager, IEmailService emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<OperationResult> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return OperationResult.Fail();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetLink = QueryHelpers.AddQueryString(request.ResetLinkAction, new Dictionary<string, string?>
            {
                ["email"] = request.Email,
                ["token"] = encodedToken
            });

            await _emailSender.SendMail(request.Email, "بازیابی رمز عبور", $"<a href='{resetLink}'>اینجا کلیک کنید</a>");

            return OperationResult.Success();
        }
    }
}
