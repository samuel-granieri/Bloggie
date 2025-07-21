using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Implementations;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeInterface blogPostLikeInterface;

        public BlogPostLikeController(IBlogPostLikeInterface blogPostLikeInterface)
        {
            this.blogPostLikeInterface = blogPostLikeInterface;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId,
            };
            
            await blogPostLikeInterface.AddLikeForBlog(model);
            
            return Ok();
            
        }
        
        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotallikesForBlogs([FromRoute] Guid blogPostid)
        {
            var totalLikes = await blogPostLikeInterface.GetTotalLikes(blogPostid);

            return Ok(totalLikes);
        }
    
    }
}
