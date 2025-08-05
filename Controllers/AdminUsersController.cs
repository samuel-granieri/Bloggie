using Bloggie.Models.ViewModels;
using Bloggie.Repositories.Implementations;
using Bloggie.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserInterface userInterface;

        public AdminUsersController(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }
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
    }
}
