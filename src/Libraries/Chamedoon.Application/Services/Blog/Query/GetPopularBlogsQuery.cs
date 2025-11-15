using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Blog.Query;

public class GetPopularBlogsQuery : IRequest<OperationResult<IReadOnlyList<BlogViewModel>>>
{
    public int Count { get; set; } = 3;
}

public class GetPopularBlogsQueryHandler : IRequestHandler<GetPopularBlogsQuery, OperationResult<IReadOnlyList<BlogViewModel>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPopularBlogsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<IReadOnlyList<BlogViewModel>>> Handle(GetPopularBlogsQuery request, CancellationToken cancellationToken)
    {
        var take = request.Count <= 0 ? 3 : request.Count;

        var popularArticles = await _context.Article
            .OrderByDescending(article => article.VisitCount)
            .ThenByDescending(article => article.Created)
            .Take(take)
            .ProjectTo<BlogViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return OperationResult<IReadOnlyList<BlogViewModel>>.Success(popularArticles);
    }
}
