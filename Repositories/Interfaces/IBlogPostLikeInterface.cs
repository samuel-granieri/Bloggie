using Bloggie.Models.Domain;

namespace Bloggie.Repositories.Interfaces
{
    public interface IBlogPostLikeInterface
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
    }
}
