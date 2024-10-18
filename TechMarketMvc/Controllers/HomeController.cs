using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechMarketMvc.Models;
using TechMarketMvc.ViewModels;

namespace TechMarketMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var model = new ProductViewModel
        {
            Computers = new List<Computer>(),
            Phones = new List<Phone>(),
            Smartwatches = new List<Smartwatch>()
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
