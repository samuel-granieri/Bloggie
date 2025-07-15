using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Bloggie.Controllers
{
    [Route("[controller]")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagInterface tagInterface;
        private readonly IBlogPostInterface blogPostInterface;


        public AdminBlogPostsController(ITagInterface tagInterface, IBlogPostInterface blogPostInterface)
        {
            this.tagInterface = tagInterface;
            this.blogPostInterface = blogPostInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagInterface.GetAllAsync();

            var model = new AddBlogPostResquest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostResquest addBlogPostResquest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostResquest.Heading,
                PageTitle = addBlogPostResquest.PageTitle,
                Content = addBlogPostResquest.Content,
                ShortDescription = addBlogPostResquest.ShortDescription,
                FeaturedImageUrl = addBlogPostResquest.FeaturedImageUrl,
                UrlHandle = addBlogPostResquest.UrlHandle,
                PublishDate = addBlogPostResquest.PublishDate,
                Author = addBlogPostResquest.Author,
                Visible = addBlogPostResquest.Visible
            };

            //Map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostResquest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagInterface.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }

            }


            blogPost.Tags = selectedTags;

            await blogPostInterface.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostInterface.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostInterface.GetAsync(id);
            var tagsDomainModel = await tagInterface.GetAllAsync();

            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishDate = blogPost.PublishDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()

                };

                return View(model);
            }

            return View(null);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishDate = editBlogPostRequest.PublishDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            //map tags to domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)

            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagInterface.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }


            blogPostDomainModel.Tags = selectedTags;

            //update info
            var updatedBlog = await blogPostInterface.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                return RedirectToAction("Edit", new { id = blogPostDomainModel.Id });
            }

            return RedirectToAction("Edit");

        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedBlogPost = await blogPostInterface.DeleteAsync(id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = deletedBlogPost.Id});;
        }

    }
}