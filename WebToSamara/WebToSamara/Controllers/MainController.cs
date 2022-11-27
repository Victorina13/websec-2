using Microsoft.AspNetCore.Mvc;
using WebToSamara.Models;
using WebToSamara.Common;
using System.Text;
using System.Security.Cryptography;

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

        public FormUrlEncodedContent GetRequestString(string methodName, string KS_ID, string HULLNO, string KR_ID)
        {
            string? message = methodName == _methods[0]
                    ? $"{{\"method\":\"{methodName}\", \"KS_ID\":{KS_ID},\"COUNT\":{20}}}"
                    : methodName == _methods[1]
                        ? $"{{\"method\":\"{methodName}\", \"KR_ID\":{KR_ID},\"day\":{DateTime.Now:dd.MM.yyyy}}}"
                        : methodName == _methods[2]
                            ? $"{{\"method\":\"{methodName}\", \"HULLNO\":{HULLNO}}}"
                            : String.Empty;


            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes($"{message}{Configuration.Secret_key}"));
            string resHash = string.Join("",hash.Select(b => b.ToString("x2")).ToList());
            
            var body = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("message", message),
                new KeyValuePair<string, string>("os", Configuration.Os),
                new KeyValuePair<string, string>("clientId", Configuration.ClientId),
                new KeyValuePair<string, string>("authKey", resHash)
            };

            return new FormUrlEncodedContent(body);
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