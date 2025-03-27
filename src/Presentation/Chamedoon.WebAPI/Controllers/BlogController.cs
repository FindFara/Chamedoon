//using Chamedoon.Application.Services.Blog.Command;
//using Chamedoon.Application.Services.Blog.Query;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Chamedoon.WebAPI.Controllers
//{
//    public class BlogController : ApiControllerBase
//    {
//        private readonly IMediator mediator;

//        public BlogController(IMediator mediator)
//        {
//            this.mediator = mediator;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddArticle([FromBody] CreateArticleCommand command)
//        {
//            return Ok(await mediator.Send(command));
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditArticle([FromBody] EditArticleCommand command)
//        {
//            return Ok(await mediator.Send(command));
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteArticle([FromBody] DeleteArticleCommand command)
//        {
//            return Ok(await mediator.Send(command));
//        }

//        [HttpPost]
//        public async Task<IActionResult> GetBlog([FromBody] GetBlogQuery command)
//        {
//            return Ok(await mediator.Send(command));
//        }
//    }
//}
