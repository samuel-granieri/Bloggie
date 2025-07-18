using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostInterface blogPostInterface;

        public BlogsController(IBlogPostInterface blogPostInterface)
        {
            this.blogPostInterface = blogPostInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await blogPostInterface.GetByUrlHandleAsync(urlHandle);

            return View(blogPost);
        }
    }
}
