using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Command;

public class ManageRegisterUserCommand : IRequest<BaseResult_VM<bool>>
{
    public RegisterUser_VM RegisterUser_VM { get; set; }
}
public class ManageRegisterUserCommandHandler : IRequestHandler<ManageRegisterUserCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly ISender sender;
    #endregion

    #region Ctor
    public ManageRegisterUserCommandHandler(IApplicationDbContext context, ISender sender)
    {
        _context = context;
        this.sender = sender;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(ManageRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var register = await sender.Send(new RegisterUserCommand { RegisterUser_VM = request.RegisterUser_VM });

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "",

        };
    }

    #endregion
}


