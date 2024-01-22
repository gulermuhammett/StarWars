using Microsoft.AspNetCore.Mvc;
using StarWars.UI.Models;
using System.Diagnostics;
using StarWars.API.Entities;
using Newtonsoft.Json;

namespace StarWars.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseURL = "https://localhost:7240";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<People> people = new List<People>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Character/GetCharacters"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    people = JsonConvert.DeserializeObject<Resultes<People>>(apiResult).Results;
                }

            }
            return View(people);
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
}
