using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
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


public class AddUserInAdminCommand : IRequest<OperationResult<bool>>
{
}
public class AddUserInAdminCommandHandler : IRequestHandler<AddUserInAdminCommand, OperationResult<bool>>
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
    public async Task<OperationResult<bool>> Handle(AddUserInAdminCommand request, CancellationToken cancellationToken)
    {
        return OperationResult<bool>.Success(true);
    }

    #endregion
}
