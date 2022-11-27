using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebToSamara.Models;
using System.Configuration;
using WebToSamara.Common;
using Newtonsoft.Json;

namespace WebToSamara.Controllers
{
    public class MainController : Controller
    {
        private List<string> _methods = new List<string>() { "getFirstArrivalToStop", "getRouteSchedule", "getTransportPosition" };
        
        public Stops StopsObj { get; set; }
        
        public Routes RoutesObj { get; set; }
        
        private readonly ILogger<MainController> _logger;

        private readonly WebConfig Configuration;

        public MainController(ILogger<MainController> logger, IConfiguration configuration)
        {
            _logger = logger;
            StopsObj = new Stops();
            RoutesObj = new Routes();
            var config = configuration.GetSection("Configuration");
            Configuration = new WebConfig(config.GetValue<string>("clientId"), config.GetValue<string>("os"), config.GetValue<string>("secret_key"), config.GetValue<string>("requestUrl"));
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