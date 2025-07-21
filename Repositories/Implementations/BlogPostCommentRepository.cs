using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories.Implementations
{
    public class BlogPostCommentRepository : IBlogPostCommentInterface
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostcomment)
        {
            await bloggieDbContext.BlogPostComment.AddAsync(blogPostcomment);   
            await bloggieDbContext.SaveChangesAsync();
            return blogPostcomment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
