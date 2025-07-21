using Bloggie.Models.Domain;

namespace Bloggie.Repositories.Interfaces
{
    public interface IBlogPostCommentInterface
    {
        Task<BlogPostComment> AddAsync(BlogPostComment comment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);
    }
}
