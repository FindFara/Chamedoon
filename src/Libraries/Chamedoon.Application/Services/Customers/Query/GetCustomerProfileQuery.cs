using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Application.Services.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Chamedoon.Application.Services.Customers.Query
{
    public class GetCustomerProfileQuery : IRequest<OperationResult<CustomerProfileViewModel>>
    {
        public required string UserName{ get; set; }
    }
    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, OperationResult<CustomerProfileViewModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper mapper;

        public GetCustomerProfileQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public async Task<OperationResult<CustomerProfileViewModel>> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.UserName == request.UserName);
            if (user is null)
            {
                return OperationResult<CustomerProfileViewModel>.Fail();
            }

            var customer = await _context.Customers
                .Where(c => c.Id == user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
            {
                return OperationResult<CustomerProfileViewModel>.Fail();
            }

            var profile = mapper.Map<CustomerProfileViewModel>(customer);
            profile.ProfileImage = ProfileImageHelper.NormalizeProfileImage(profile.ProfileImage);

            return OperationResult<CustomerProfileViewModel>
                .Success(profile);
        }
    }
}
