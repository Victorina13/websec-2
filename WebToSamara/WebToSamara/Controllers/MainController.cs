using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebToSamara.Models;

namespace WebToSamara.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Views/Main/Index.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}