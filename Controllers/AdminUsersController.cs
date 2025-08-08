using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Implementations;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    [Authorize(Roles="Admin, User")]
    public class AdminUsersController : Controller
    {
        private readonly IUserInterface userInterface;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserInterface userInterface, UserManager<IdentityUser> userManager)
        {
            this.userInterface = userInterface;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userInterface.GetAll();


            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email
                });
            }      



            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            //criar usuario
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            //adicionar role a este usuario
            if (identityResult.Succeeded && identityResult is not null)
            {
                //lista iniciada com usuario padrao
                var roles = new List<string>() { "User" };

                if(request.AdminRoleCheckbox)
                {
                    //lista com usario padrao e admin se for selecionado esta opcao tambem
                    roles.Add("Admin");
                }

                //criando role para o usuario
                identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                if(identityResult.Succeeded && identityResult is not null)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("list", "AdminUsers");
                }    
            }
            return View();
        }
    }
}
