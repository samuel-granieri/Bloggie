﻿using Bloggie.Data;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories.Implementations
{
    public class UserRepository : IUserInterface
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository( AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
           var users =  await authDbContext.Users.ToListAsync();

            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }

            return users;   
        }
    }
}
