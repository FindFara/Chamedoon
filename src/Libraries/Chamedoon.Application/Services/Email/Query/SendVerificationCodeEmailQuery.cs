using Chamedoon.Application.Common.Models;
using MediatR;

namespace Chamedoon.Application.Services.Email.Query
{
    public class SendVerificationCodeEmailQuery : IRequest<OperationResult<bool>>
    {
        public required string Email { get; set; }
        public required string VerificationCode { get; set; }
    }

    public class SendVerificationCodeEmailQueryHandler : IRequestHandler<SendVerificationCodeEmailQuery, OperationResult<bool>>
    {
        private readonly IEmailService _emailService;

        public SendVerificationCodeEmailQueryHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<OperationResult<bool>> Handle(SendVerificationCodeEmailQuery request, CancellationToken cancellationToken)
        {
            var subject = "کد تأیید ثبت‌نام";
            var content = $"<p>کد تأیید شما برای ورود به چمدون:</p><h2 style=\"letter-spacing:6px;\">{request.VerificationCode}</h2><p>این کد تا چند دقیقه آینده معتبر است.</p>";

            try
            {
                await _emailService.SendMail(request.Email, subject, content);
                return OperationResult<bool>.Success(true);
            }
            catch
            {
                return OperationResult<bool>.Fail("ارسال کد تأیید با مشکل مواجه شد.");
            }
        }
    }
}
