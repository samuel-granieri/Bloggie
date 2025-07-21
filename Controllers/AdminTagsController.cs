using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Implementations;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bloggie.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminTagsController : Controller
    {

        private readonly ITagInterface tagInterface;

        public AdminTagsController(ITagInterface tagInterface)
        {
            this.tagInterface = tagInterface;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagResquest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagInterface.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagInterface.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagInterface.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagInterface.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //Show success notification
            }
            else
            {
                //Show error notification
            }
           
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deleteTag = await tagInterface.DeleteAsync(editTagRequest.Id);

            if (deleteTag != null)
            {
                //Show sucess notification
                return RedirectToAction("List");
            }

            //Show error notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}