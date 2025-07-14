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

        [HttpGet]
        public async Task<IActionResult> List()
        {
           var blogPosts = await blogPostInterface.GetAllAsync();
            return View();
        }




    }
}