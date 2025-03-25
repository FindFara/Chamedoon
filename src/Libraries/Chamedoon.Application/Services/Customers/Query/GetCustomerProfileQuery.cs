﻿using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
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

            var customer = await _context.Customers
                .Where(c => c.Id == user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null || string.IsNullOrEmpty(customer.ProfileImage))
            {
                return OperationResult<CustomerProfileViewModel>.Fail();
            }

            return OperationResult<CustomerProfileViewModel>
                .Success(mapper.Map< CustomerProfileViewModel> (customer));
        }
    }
}
