using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Chamedoon.Application.Services.Blog.Query;
public class GetBlogByPaginationQuery : IRequest<OperationResult<List<BlogViewModel>>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? FilterTitle { get; set; }
}
public class GetBlogByPaginationQueryHandler : IRequestHandler<GetBlogByPaginationQuery, OperationResult<List<BlogViewModel>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;

    public GetBlogByPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    public async Task<OperationResult<List<BlogViewModel>>> Handle(GetBlogByPaginationQuery request, CancellationToken cancellationToken)
    {
        var result =  _context.Article
                            .OrderByDescending(b => b.Created)
                            .Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);

        if (!string.IsNullOrEmpty(request.FilterTitle))
        {
            result = result.Where(u => u.ArticleTitle.Contains(request.FilterTitle));
        }

        var blogs = mapper.Map<List<BlogViewModel>>(await result.ToListAsync());

        if (blogs != null)
            return OperationResult<List<BlogViewModel>>.Success(blogs);

        return OperationResult<List<BlogViewModel>>.Fail();

    }
}
