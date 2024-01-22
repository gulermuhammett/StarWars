using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models;
using System.Diagnostics;

namespace StarWars.UI.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ILogger<FilmsController> _logger;
        string baseURL = "https://localhost:7240";

        public FilmsController(ILogger<FilmsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Films> films = new List<Films>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetAllFilms"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    films = JsonConvert.DeserializeObject<Resultes<Films>>(apiResult).Results;
                }

            }
            return View(films);
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

