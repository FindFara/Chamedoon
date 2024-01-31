using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Query;

public class GetUserQuery : IRequest<BaseResult_VM<User>>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserQuery, BaseResult_VM<User>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUserByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User
             .SingleOrDefaultAsync(u =>
                                   u.Id == request.Id ||
                                   u.NormalizedUserName ==(request.UserName?? "").ToUpper()  ||
                                   u.NormalizedEmail == (request.Email ?? "").ToUpper());
        if (user is null)
            return new BaseResult_VM<User>
            {
                Code = -1,
                Message = "کاربری یافت نشد",
            };

        return new BaseResult_VM<User>
        {
            Result = mapper.Map<User>(user) ,
            Code = 0,
            Message = "کاربر با موفقیت یافت شد",
        };
    }
    #endregion
}
