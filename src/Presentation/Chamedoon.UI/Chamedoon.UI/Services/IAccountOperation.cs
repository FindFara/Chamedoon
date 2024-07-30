using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Chamedoon.UI.Services
{
    internal interface IAccountOperation
    {
        internal Task<OperationResult<bool>> Register(RegisterUser_VM registerUser);

    }
    internal class AccountOperation : IAccountOperation
    {
        private readonly IMediator _mediator;

        public AccountOperation(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<OperationResult<bool>> Register(RegisterUser_VM registerUser)
        {
            return await _mediator.Send(new ManageRegisterUserCommand { RegisterUser = registerUser });
        }
    }
}
