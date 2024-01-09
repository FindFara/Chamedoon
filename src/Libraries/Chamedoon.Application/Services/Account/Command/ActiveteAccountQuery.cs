using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Command;

public class ActiveteAccountQuery : IRequest<BaseResult_VM<bool>>
{
    public string? EmailActivCode { get; set; }
}
public class ActiveteAccountQueryHandler : IRequestHandler<ActiveteAccountQuery, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public ActiveteAccountQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(ActiveteAccountQuery request, CancellationToken cancellationToken)
    {
        //var user = await _context.User.FirstAsync( u=>u. == request.EmailActivCode);
        // if(user == null)
        // {

        // }

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "",

        };
    }

    #endregion
}


