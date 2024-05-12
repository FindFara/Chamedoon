using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;

public class EditUserAdminCommand : IRequest<OperationResult<bool>>
{
    public AdminEditUser_VM User { get; set; }
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
        
        var user = await _context.User.FindAsync(request.User.Id, cancellationToken);

        if (user == null)
            return OperationResult<bool>.Fail("کاربر یافت نشد");  
        
        if (request.User != null) 
            _mapper.Map(request.User, user); 

        var updateResult = await _userManager.UpdateAsync(user);

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
