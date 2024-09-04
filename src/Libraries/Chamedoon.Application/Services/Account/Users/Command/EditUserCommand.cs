using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;

public class EditUserCommand : IRequest<OperationResult<bool>>
{
    public EditUser_VM User{ get; set; }
}
public class EditUserCommandHandler : IRequestHandler<EditUserCommand, OperationResult<bool>>
{
    #region Properties
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    #endregion

    #region Constructor
    public EditUserCommandHandler(IApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = _context.User.AsNoTracking().FirstOrDefault(u=>u.Id == request.User.Id);
        if (user == null)
            return OperationResult<bool>.Fail("کاربر یافت نشد");

        var updateUser = _mapper.Map(request.User,user);
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
