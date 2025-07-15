using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageInterface imageInterface;

        public ImagesController(IImageInterface imageInterface)
        {
            this.imageInterface = imageInterface;
        }      

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {

            var imageUrl = await imageInterface.UploadAsync(file);

            if (imageUrl == null) 
            {
                return Problem("Somenthing went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageUrl});
        }
    }
}