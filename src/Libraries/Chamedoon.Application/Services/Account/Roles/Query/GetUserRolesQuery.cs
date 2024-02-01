using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Query
{

    public class GetUserRolesQuery : IRequest<BaseResult_VM<IList<string>>>
    {
        public long? Id { get; set; }
        public string? UserName { get; set; }
    }
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, BaseResult_VM<IList<string>>>
    {
        #region Property
        private readonly UserManager<User> userManager;
        private readonly IMediator mediator;
        #endregion

        #region Ctor
        public GetUserRolesQueryHandler(UserManager<User> userManager, IMediator mediator)
        {
            this.userManager = userManager;
            this.mediator = mediator;
        }
        #endregion

        #region Method
        public async Task<BaseResult_VM<IList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await mediator.Send(new GetUserQuery { UserName = request.UserName, Id = request.Id });
            if (user.Result is null)
                return new BaseResult_VM<IList<string>> { Code = user.Code, Message = user.Message };

            IList<string>? roles = await userManager.GetRolesAsync(user.Result);
            if (roles is null || roles.Count is 0)
                return new BaseResult_VM<IList<string>> { Code = -1, Message = "برای کاربر نقشی یافت نشد" };

            return new BaseResult_VM<IList<string>>
            {
                Result = roles,
                Code = 0,
                Message = "با موفقیت فراخوانی شد",

            };
        }

        #endregion
    }
}
