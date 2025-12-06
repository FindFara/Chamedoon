using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Customers.Command;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class ManageRegisterUserCommand : IRequest<OperationResult<bool>>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}
public class ManageRegisterUserCommandHandler : IRequestHandler<ManageRegisterUserCommand, OperationResult<bool>>
{
    #region Property
    private readonly IMediator mediator;
    private readonly IMemoryCache memoryCache;
    #endregion

    #region Ctor
    public ManageRegisterUserCommandHandler(IMediator mediator, IMemoryCache memoryCache)
    {
        this.mediator = mediator;
        this.memoryCache = memoryCache;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(ManageRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"register:{request.RegisterUser.Email.ToLowerInvariant()}";
        if (!memoryCache.TryGetValue<string>(cacheKey, out var storedCode) || storedCode != request.RegisterUser.VerificationCode)
            return OperationResult<bool>.Fail("کد تایید نامعتبر است یا منقضی شده است.");

        memoryCache.Remove(cacheKey);

        //Check Duplicated Email
        var checkEmail = await mediator.Send(new CheckDuplicatedEmailQuery { Email = request.RegisterUser.Email });
        if (checkEmail.IsSuccess is false)
            return OperationResult<bool>.Fail(checkEmail.Message);

        //Register User
        var regisrer = await mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser });
        if (regisrer.IsSuccess is false)
            return OperationResult<bool>.Fail(regisrer.Message);

        var addCustomer = await mediator.Send(new AddCustomerCommand { Id = regisrer.Result});
        if (addCustomer.IsSuccess is false)
            return OperationResult<bool>.Fail(addCustomer.Message);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}


