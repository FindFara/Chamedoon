using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Configs;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace Chamedoon.Application.Services.Email.Query
{
    public class SendTokenToUserEmailQuery : IRequest<OperationResult<bool>>
    {
        public required string UserName { get; set; }
    }
    public class SendTokenToUserEmailQueryHandler : IRequestHandler<SendTokenToUserEmailQuery, OperationResult<bool>>
    {
        #region Property
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IEmailService emailService;
        private readonly string AppUrl;

        #endregion

        #region Ctor
        public SendTokenToUserEmailQueryHandler(UserManager<User> userManager,
            IConfiguration configuration,
            IMediator mediator,
            IEmailService emailService,
            IOptions<UrlsConfig> urlConfig)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mediator = mediator;
            this.emailService = emailService;
            AppUrl = urlConfig.Value.AppUrl;
        }
        #endregion

        #region Method
        public async Task<OperationResult<bool>> Handle(SendTokenToUserEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await mediator.Send(new GetUserQuery { UserName = request.UserName });
            if (user.IsSuccess is false || user.Result is null)
                return OperationResult<bool>.Fail(user.Message);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user.Result);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string url = $"{AppUrl}/auth/Confirmemail?userid={user.Result.Id}&token={encodedToken}";
            try
            {
               await emailService.SendMail(user.Result.Email
                 , "Confirm Your Email"
                 , "<h1>Wellcome </h1>"
                 + $"<p>confirme your email<a href ='{url}'> clicking here</a></p> ");
            }
            catch (Exception)
            {
                return OperationResult<bool>.Fail();
            }
            return OperationResult<bool>.Success(true);
        }

        #endregion
    }
}
