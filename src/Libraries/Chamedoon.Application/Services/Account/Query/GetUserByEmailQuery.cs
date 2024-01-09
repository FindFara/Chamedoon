using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Query;

public class GetUserByEmailQuery : IRequest<BaseResult_VM<UserDetails_VM>>
{
    public string Email { get; set; }
}
public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, BaseResult_VM<UserDetails_VM>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUserByEmailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<UserDetails_VM>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User.SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return new BaseResult_VM<UserDetails_VM>
            {
                Code = -1,
                Message = "کاربری با این ایمیل یافت نشد",
            };
        }

        return new BaseResult_VM<UserDetails_VM>
        {
            Result = mapper.Map<UserDetails_VM>(user),
            Code = 0,
            Message = "کاربر با موفقیت یافت شد",
        };
    }
    #endregion
}


