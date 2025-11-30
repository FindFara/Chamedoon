using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Chamedoon.Application.Services.Immigration
{
    /// <summary>
    /// درخواست MediatR برای محاسبه امتیاز مهاجرت. این کلاس داده‌های ورودی
    /// متقاضی را در بر می‌گیرد و خروجی آن <see cref="ImmigrationResult"/> خواهد بود.
    /// </summary>
    public class ImmigrationQuery : IRequest<ImmigrationResult>
    {
        /// <summary>
        /// اطلاعات ورودی متقاضی.
        /// </summary>
        public ImmigrationInput Input { get; set; } = new ImmigrationInput();
    }

    /// <summary>
    /// هندلر MediatR که امتیاز مهاجرت را با استفاده از <see cref="ImmigrationScoringService"/> محاسبه می‌کند.
    /// </summary>
    public class ImmigrationQueryHandler : IRequestHandler<ImmigrationQuery, ImmigrationResult>
    {
        private readonly ImmigrationScoringService _scoringService;

        /// <summary>
        /// سازنده با تزریق سرویس امتیازدهی.
        /// </summary>
        public ImmigrationQueryHandler(ImmigrationScoringService scoringService)
        {
            _scoringService = scoringService;
        }

        public async Task<ImmigrationResult> Handle(ImmigrationQuery request, CancellationToken cancellationToken)
        {
            var result = await _scoringService.CalculateImmigrationAsync(request.Input, cancellationToken);
            return result;
        }
    }
}