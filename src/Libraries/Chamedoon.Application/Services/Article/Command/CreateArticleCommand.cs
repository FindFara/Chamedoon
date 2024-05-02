using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Article.Command;

public class CreateArticleCommand : IRequest<OperationResult<bool>>
{
}
public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public CreateArticleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {


        return OperationResult<bool>.Success(true);
    }

    #endregion
}
