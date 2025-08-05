using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace Bloggie.Repositories.Interfaces
{
    public interface IUserInterface
    {
        public Task<IEnumerable<IdentityUser>> GetAll();
    }
}
