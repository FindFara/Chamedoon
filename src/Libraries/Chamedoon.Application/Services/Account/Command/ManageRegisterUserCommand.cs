using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.Authentication;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Command;

public class ManageRegisterUserCommand : IRequest<ResponseRegisterUser_VM>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}
public class ManageRegisterUserCommandHandler : IRequestHandler<ManageRegisterUserCommand, ResponseRegisterUser_VM>
{
    #region Property
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public ManageRegisterUserCommandHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<ResponseRegisterUser_VM> Handle(ManageRegisterUserCommand request, CancellationToken cancellationToken)
    {
        //Check Duplicated Email
        var checkEmail = await mediator.Send(new CheckDuplicatedEmailQuery { Email = request.RegisterUser.Email });
        if (checkEmail.Code is not 0)
            return new ResponseRegisterUser_VM
            {
                Code = checkEmail.Code,
                Message = checkEmail.Message,
            };

        //Check Duplicated UserName
        var checkUserName = await mediator.Send(new CheckDuplicatedUserNameQuery { UserName = request.RegisterUser.UserName });
        if (checkUserName.Code is not 0)
            return new ResponseRegisterUser_VM
            {
                Code = checkUserName.Code,
                Message = checkUserName.Message,
            };

        //Register User
        var regisrer = await mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser });







        return regisrer;
    }

    #endregion
}


