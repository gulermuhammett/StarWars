using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;

namespace StarWars.UI.Controllers
{
    public class StarshipController : Controller
    {
        private readonly ILogger<StarshipController> _logger;
        string baseURL = "https://localhost:7240";

        public StarshipController(ILogger<StarshipController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> GetAllStarshipAsync()
        {
            List<Starships> species = new List<Starships>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Starships/GetStarships"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Starships>>(apiResult).Results;
                }

            }
            return View(species);
        }
    }
}
