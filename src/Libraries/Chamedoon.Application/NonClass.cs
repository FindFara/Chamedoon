using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application;

public class NonClass : IRequest<BaseResult_VM<bool>>
{
}
public class Handler : IRequestHandler<NonClass, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(NonClass request, CancellationToken cancellationToken)
    {
        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "",

        };
    }

    #endregion
}


