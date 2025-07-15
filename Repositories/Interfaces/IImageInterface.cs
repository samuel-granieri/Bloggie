using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bloggie.Repositories.Interfaces
{
    public interface IImageInterface
    {
        Task<string> UploadAsync(IFormFile file);
    }
}