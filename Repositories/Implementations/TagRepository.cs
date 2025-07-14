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
    public class TagRepository : ITagInterface
    {
        private readonly BloggieDbContext bloggieDbContext;
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = await bloggieDbContext.Tags.ToListAsync();

            return tags;
        }
        public async Task<Tag> GetAsync(Guid id)
        {
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag != null)
            {
                return tag;
            }

            return null;
        }
        public async Task<Tag> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;


        }
        public async Task<Tag> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        } 
    }
}