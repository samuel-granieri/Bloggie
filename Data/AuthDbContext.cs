using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        // Construtor que recebe opções e repassa para o base
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        // DbSets de outras entidades aqui
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            

            var adminRoleId = "ee352320-bed0-4e49-946c-de230390ee5a";
            var superAdminRoleId = "6a6e86a6-42d8-4230-bce6-11566cef319e";
            var userRoleId = "56cb42ec-eeda-4ac1-99ec-fbea0d3b4f61";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name= "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name= "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }

            };

            //Criar Roles
            builder.Entity<IdentityRole>().HasData(roles);


            

            var superAdminId = "3f061dcf-0110-478d-b42b-9eccffeff7e7";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };

            //Hash da senha
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            //Criar Usuario
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Adicionar roles para o usuario superAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            //Atribuir roles ao usuario
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);




        }



    }
}
