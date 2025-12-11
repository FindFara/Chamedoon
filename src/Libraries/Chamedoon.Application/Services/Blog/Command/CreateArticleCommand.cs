using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;

namespace Chamedoon.Application.Services.Blog.Command;

public class CreateArticleCommand : IRequest<OperationResult<bool>>
{
    public ArticleViewModel? Article { get; set; }
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
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}
