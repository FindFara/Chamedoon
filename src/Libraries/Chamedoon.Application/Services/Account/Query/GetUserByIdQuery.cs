using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Account.Query;

public class GetUserByIdQuery : IRequest<BaseResult_VM<User>>
{
    public required long Id { get; set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, BaseResult_VM<User>>
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
    public async Task<BaseResult_VM<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User.SingleOrDefaultAsync(u => u.Id == request.Id);

        if (user == null)
        {
            return new BaseResult_VM<User>
            {
                Code = -1,
                Message = "کاربری با این نام کاربری یافت نشد",
            };
        }

        return new BaseResult_VM<User>
        {
            Result = user,
            Code = 0,
            Message = "کاربر با موفقیت یافت شد",
        };
    }
    #endregion
}
