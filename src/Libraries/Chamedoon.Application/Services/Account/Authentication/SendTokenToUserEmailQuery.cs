using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Chamedoon.Application.Services.Account.Authentication
{
    public class SendTokenToUserEmailQuery : IRequest<BaseResult_VM<bool>>
    {
        public required User User { get; set; }
    }
    public class SendTokenToUserEmailQueryHandler : IRequestHandler<SendTokenToUserEmailQuery, BaseResult_VM<bool>>
    {
        #region Property
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IEmailService emailService;


        #endregion

        #region Ctor
        public SendTokenToUserEmailQueryHandler(UserManager<User> userManager, IConfiguration configuration, IMediator mediator, IEmailService emailService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mediator = mediator;
            this.emailService = emailService;
        }
        #endregion

        #region Method
        public async Task<BaseResult_VM<bool>> Handle(SendTokenToUserEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await mediator.Send(new GetUserByUserNameQuery { UserName = request.User.UserName });
            if (user.Code is not 0)
                return new BaseResult_VM<bool>
                {
                    Code = user.Code,
                    Message = user.Message,
                };

            var token = await userManager.GenerateEmailConfirmationTokenAsync(request.User);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{configuration["AppUrl"]}/api/Account/Confirmemail?userid={user.Result.Id}&token={validToken}";

            //TODO :  sing up in sendgrid.com
            //await emailService.SendEmailAsync(user.Result.Email
            //     , "Confirm Your Email"
            //     , "<h1>Wellcome </h1>"
            //     + $"<p>confirme your email<a href ='{url}'> clicking here</a></p> ");


            return new BaseResult_VM<bool>
            {
                Result = true,
                Code = 0,
                Message = "",

            };
        }

        #endregion
    }
}
