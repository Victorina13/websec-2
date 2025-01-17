﻿using Microsoft.AspNetCore.Mvc;
using WebToSamara.Models;
using WebToSamara.Common;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNetCore.Mvc.Formatters;

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
            StopsObj.LoadFromXml();
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

        [Route("Main/GetFavorites")]
        public string GetFavorites()
        {
            try
            {
                Stops? favorites;
                using (var reader = new StreamReader("favorites.json"))
                {
                    favorites = JsonConvert.DeserializeObject<Stops>(reader.ReadToEnd());
                    if (favorites == null || favorites.StopsList.Count == 0)
                    {
                        favorites = new Stops();
                        favorites.StopsList.Clear();
                    }
                    return JsonConvert.SerializeObject(new
                    {
                        isSuccess = true,
                        data = favorites
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { isSuccess = false, error = ex.Message });
            }
        }

        [Route("Main/AddToFavorite/{KS_ID}")]
        public string AddToFavorite(long KS_ID)
        {
            try
            {
                Stops? favorites;
                using (var reader = new StreamReader("favorites.json"))
                {
                    favorites = JsonConvert.DeserializeObject<Stops>(reader.ReadToEnd());
                }
                if (favorites == null || favorites.StopsList.Count == 0)
                {
                    favorites = new Stops();
                    favorites.StopsList.Clear();
                    favorites.StopsList.Add(StopsObj.StopsList.First(x => x.KS_ID == KS_ID));
                }
                else
                {
                    Stop stop = StopsObj.StopsList.First(x => x.KS_ID == KS_ID);
                    if (!favorites.StopsList.Any(x => x.KS_ID == KS_ID))
                    {
                        favorites.StopsList.Add(stop);
                    }
                    else
                    {
                        throw new Exception("Остановка уже добавлена в избранные!");
                    }
                }
                using (var writer = new StreamWriter("favorites.json"))
                {
                    writer.Write(JsonConvert.SerializeObject(favorites));
                }
                return JsonConvert.SerializeObject(new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { isSuccess = false, error = ex.Message });
            }
        }

        [Route("Main/DeleteFromFavorite/{KS_ID}")]
        public string DeleteFromFavorite(long KS_ID)
        {
            try
            {
                Stops? favorites;
                using (var reader = new StreamReader("favorites.json"))
                {
                    favorites = JsonConvert.DeserializeObject<Stops>(reader.ReadToEnd());
                }
                if (favorites == null || favorites.StopsList.Count == 0)
                {
                    throw new Exception("Список избранных остановок пуст!");
                }
                else
                {
                    Stop stop = StopsObj.StopsList.First(x => x.KS_ID == KS_ID);
                    if (favorites.StopsList.Any(x => x.KS_ID == KS_ID))
                    {
                        favorites.StopsList.RemoveAt(favorites.StopsList.FindIndex(x => x.KS_ID == KS_ID));
                    }
                    else
                    {
                        throw new Exception("Остановка не была добавлена в избранные!");
                    }
                }
                using (var writer = new StreamWriter("favorites.json"))
                {
                    writer.Write(JsonConvert.SerializeObject(favorites));
                }
                return JsonConvert.SerializeObject(new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { isSuccess = false, error = ex.Message });
            }
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