using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Models;
using Bloggie.Repositories.Interfaces;
using Bloggie.Models.ViewModels;

namespace Bloggie.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostInterface blogPostInterface;
    private readonly ITagInterface tagInterface;

    public HomeController(ILogger<HomeController> logger, IBlogPostInterface blogPostInterface, ITagInterface tagInterface)
    {
        _logger = logger;
        this.blogPostInterface = blogPostInterface;
        this.tagInterface = tagInterface;
    }

    public async Task<IActionResult> Index()
    {
        var blogPosts = await blogPostInterface.GetAllAsync();
        var tags = await tagInterface.GetAllAsync();

        var model = new HomeViewModel
        {
            BlogPosts = blogPosts,
            Tags = tags
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
