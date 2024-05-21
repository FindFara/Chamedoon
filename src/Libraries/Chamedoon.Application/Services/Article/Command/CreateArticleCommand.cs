using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Article.ViewModel;
using Chamedoon.Domin.Entity.Blogs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Article.Command;

public class CreateArticleCommand : IRequest<OperationResult<bool>>
{
    public Article_VM? Article  { get; set; }
}
public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public CreateArticleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
       var article = mapper.Map<Domin.Entity.Blogs.Article>(request.Article);
        _context.Article.Add(article);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}
