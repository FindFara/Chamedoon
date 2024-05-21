using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;


public class CreateUserInAdminCommand : IRequest<OperationResult<bool>>
{
    public AdminCreateOrEditUser_VM User { get; set; }
}
public class AddUserInAdminCommandHandler : IRequestHandler<CreateUserInAdminCommand, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public AddUserInAdminCommandHandler(IApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
    {
        _context = context;
        this.userManager = userManager;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CreateUserInAdminCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(request.User);
        var createUser = await userManager.CreateAsync(user);
        if (createUser.Succeeded)
        {
            return OperationResult<bool>.Success(true);
        }
        return OperationResult<bool>.Fail("مشکلی در فرایند ساخت کاربر به وجود آمده است");

    }

    #endregion
}
