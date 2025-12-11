using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Application.Services.Customers;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Customers.Query;

public class GetUserAndCustomerDetailsQuery : IRequest<OperationResult<CustomerDetailsViewModel>>
{
    public string? UserName { get; set; }
}
public class GetUserAndCustomerDetailsHandler : IRequestHandler<GetUserAndCustomerDetailsQuery, OperationResult<CustomerDetailsViewModel>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUserAndCustomerDetailsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method

    async Task<OperationResult<CustomerDetailsViewModel>> IRequestHandler<GetUserAndCustomerDetailsQuery, OperationResult<CustomerDetailsViewModel>>.Handle(GetUserAndCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User
             .AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedUserName == (request.UserName ?? "").ToUpper());

        if (user is null)
            return OperationResult<CustomerDetailsViewModel>.Fail("کاربری با مشخصات واد شده یافت نشد");

        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c=>c.Id == user.Id) ?? new Customer();

        var CustomerDitails = mapper.Map<CustomerDetailsViewModel>(customer);
        CustomerDitails.ProfileImage = ProfileImageHelper.NormalizeProfileImage(CustomerDitails.ProfileImage);
        CustomerDitails.User = mapper.Map<UserDetails_VM>(user);

        return OperationResult<CustomerDetailsViewModel>.Success(CustomerDitails);
    }
    #endregion
    
}
