using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostInterface blogPostInterface;
        private readonly IBlogPostLikeInterface blogPostLikeInterface;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentInterface blogPostCommentInterface;

        public BlogsController(
            IBlogPostInterface blogPostInterface, 
            IBlogPostLikeInterface blogPostLikeInterface,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentInterface blogPostCommentInterface
            )
        {
            this.blogPostInterface = blogPostInterface;
            this.blogPostLikeInterface = blogPostLikeInterface;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentInterface = blogPostCommentInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await blogPostInterface.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();

            if (blogPost != null)
            {
                
                var totalLikes = await blogPostLikeInterface.GetTotalLikes(blogPost.Id);

                if(signInManager.IsSignedIn(User))
                {
                    var likesForBlog = await blogPostLikeInterface.GetLikesForBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);

                    if(userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                var blogCommentsDomainModel = await blogPostCommentInterface.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishDate = blogPost.PublishDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = urlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if(signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await blogPostCommentInterface.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
            
        }
    }
}
