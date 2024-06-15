using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;

public class EditUserAdminCommand : IRequest<OperationResult<bool>>
{
    public AdminCreateOrEditUser_VM User { get; set; }
}
public class EditUserAdminCommandHandler : IRequestHandler<EditUserAdminCommand, OperationResult<bool>>
{
    #region Properties
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    #endregion

    #region Constructor
    public EditUserAdminCommandHandler(IApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(EditUserAdminCommand request, CancellationToken cancellationToken)
    {

        var user = _context.User.AsNoTracking().FirstOrDefault(u=>u.Id == request.User.Id);

        if (user == null)
            return OperationResult<bool>.Fail("کاربر یافت نشد");

        var updateUser = _mapper.Map<User>(user);
        var updateResult = await _userManager.UpdateAsync(updateUser);

        if (!updateResult.Succeeded)
        {
            return OperationResult<bool>.Fail(
                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
        }
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<bool>.Success(true);
    }
    #endregion
}
