using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Permissions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Roles.Command;

public class AddRoleCommand : IRequest<BaseResult_VM<bool>>
{
    public required string RoleTitle { get; set; }
}
public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly RoleManager<Role> roleManager;
    #endregion

    #region Ctor
    public AddRoleCommandHandler(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var existsRole = await roleManager.RoleExistsAsync(request.RoleTitle ?? "");
        if (existsRole is false)
        {
            var role = new Role
            {
                Name = request.RoleTitle
            };
            var addRole = await roleManager.CreateAsync(role);
            if (addRole != null)
            {
                return new BaseResult_VM<bool>
                {
                    Result = true,
                    Code = 0,
                    Message = "Successful",
                };
            }
        }
        return new BaseResult_VM<bool>
        {
            Result = false,
            Code = -1,
            Message = "Error occurred",
        };
    }

    #endregion
}


