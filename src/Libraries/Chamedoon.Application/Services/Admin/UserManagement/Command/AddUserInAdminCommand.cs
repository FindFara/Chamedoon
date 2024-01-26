using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;


public class AddUserInAdminCommand : IRequest<BaseResult_VM<bool>>
{
}
public class AddUserInAdminCommandHandler : IRequestHandler<AddUserInAdminCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> userManager;
    #endregion

    #region Ctor
    public AddUserInAdminCommandHandler(IApplicationDbContext context , UserManager<User> userManager)
    {
        _context = context;
        this.userManager = userManager;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(AddUserInAdminCommand request, CancellationToken cancellationToken)
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
