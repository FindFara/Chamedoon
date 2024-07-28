using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace Chamedoon.UI.Services
{
    public class IAccountOperation
    {
        private readonly IMediator _mediator;

        public IAccountOperation(IMediator mediator)
        {
            _mediator = mediator;
        }

        internal async Task<bool> Register(RegisterUser_VM registerUser)
        {
            var regisrer = await _mediator.Send(new ManageRegisterUserCommand { RegisterUser = registerUser });
            return regisrer.Result;
        }
    }
}
