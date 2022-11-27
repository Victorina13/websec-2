using Microsoft.AspNetCore.Mvc;
using WebToSamara.Models;
using WebToSamara.Common;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Web;

namespace WebToSamara.Controllers
{
    public class MainController : Controller
    {
        private List<string> _methods { get; } = new() { "getFirstArrivalToStop", "getRouteSchedule", "getTransportPosition" };
        
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

        public IActionResult Route()
        {
            return View("Views/Main/Route.cshtml");
        }

        [Route("Main/GetStops")]
        public string GetStops()
        {
            return JsonConvert.SerializeObject(new { isSuccess = true, data = StopsObj.StopsList });
        }

        [Route("Main/GetSchedule/{KS_ID}")]
        public string GetSchedule(long KS_ID)
        {
            var client = new HttpClient();

            var response = client.PostAsync(
                                        Configuration.RequestUrl,
                                        GetRequestString(_methods[0], KS_ID.ToString(), string.Empty, string.Empty)
                                            ).Result;

            if (response.IsSuccessStatusCode)
            {
                var respString = response.Content.ReadAsStringAsync().Result;
                var arrival = JsonConvert.DeserializeObject<Arrival>(respString)!;
                foreach (var transport in arrival.arrival)
                {
                    transport.NextStopName = HttpUtility.UrlDecode(transport.NextStopName);
                    transport.Type = HttpUtility.UrlDecode(transport.Type);
                }
                return JsonConvert.SerializeObject(new { isSuccess = true, data = arrival });
            }
            return JsonConvert.SerializeObject(new { isSuccess = false, errorCode = response.StatusCode });
        }

        [Route("Main/GetRoute/{KR_ID}")]
        public string GetRoute(long KR_ID)
        {
            return JsonConvert.SerializeObject(new { isSuccess = true, data = RoutesObj.RoutesList.FirstOrDefault(x => x.KR_ID == KR_ID) });
        }

        [Route("Main/GetTransportPosition/{HULLNO}")]
        public string GetTransportPosition(long HULLNO)
        {
            var client = new HttpClient();

            var response = client.PostAsync(
                                        Configuration.RequestUrl,
                                        GetRequestString(_methods[2], string.Empty, HULLNO.ToString(), string.Empty)
                                            ).Result;
            if (response.IsSuccessStatusCode)
            {
                var respString = response.Content.ReadAsStringAsync().Result;
                var transportPosition = JsonConvert.DeserializeObject<TransportPosition>(respString);

                return JsonConvert.SerializeObject(new { isSuccess = true, data = transportPosition });
            }

            return JsonConvert.SerializeObject(new { isSuccess = false, errorCode = response.StatusCode });
        }

        public IActionResult Index()
        {
            return View("Views/Main/Index.cshtml");
        }
    }
}