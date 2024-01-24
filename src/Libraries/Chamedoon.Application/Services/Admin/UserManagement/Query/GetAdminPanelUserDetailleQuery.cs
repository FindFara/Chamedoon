using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query;

public class GetAdminPanelUserDetailleQuery : IRequest<BaseResult_VM<AdminPanelUserDetaile_VM>>
{
    public long UserId { get; set; }
}
public class GetAdminPanelUserDetailleQueryHandler : IRequestHandler<GetAdminPanelUserDetailleQuery, BaseResult_VM<AdminPanelUserDetaile_VM>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetAdminPanelUserDetailleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper=mapper;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<AdminPanelUserDetaile_VM>> Handle(GetAdminPanelUserDetailleQuery request, CancellationToken cancellationToken)
    {
        User? user = await context.User.SingleOrDefaultAsync(u => u.Id == request.UserId);
        if (user is null)
            return new BaseResult_VM<AdminPanelUserDetaile_VM> { Code = -1 };
        
        return new BaseResult_VM<AdminPanelUserDetaile_VM>
        {
            Result = mapper.Map<AdminPanelUserDetaile_VM>(user),
            Code = 0,
            Message = "Successful",
        };
    }

    #endregion
}
