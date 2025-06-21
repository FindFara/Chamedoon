using AutoMapper;
using Chamedoon.Application.Services.Blog.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers
{
    public class BlogController: Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public BlogController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(int PageIndex , string FilterTitle = "")
        {
            var blogs = await mediator.Send(new GetBlogByPaginationQuery { PageIndex = PageIndex  ,FilterTitle= FilterTitle });
            return View(blogs.Result);
        }
    }
}
