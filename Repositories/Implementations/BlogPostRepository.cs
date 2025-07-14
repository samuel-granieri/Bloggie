using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostInterface
    {
        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();

            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var blogPosts = await bloggieDbContext.BlogPosts.ToListAsync();

            if (blogPosts != null)
            {
                return blogPosts;
            }

            return null;
            

        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}