using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using MediatR;

namespace Chamedoon.Application.Services.Customers.Command
{
    public class AddCustomerCommand : IRequest<OperationResult>
    {
        public long Id { get; set; }
    }
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, OperationResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddCustomerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            _context.Customers.Add(new Customer { Id = request.Id });
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
    }
}
