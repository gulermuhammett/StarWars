using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;

namespace StarWars.UI.Controllers
{
    public class PlanetsController : Controller
    {
        private readonly ILogger<PlanetsController> _logger;
        string baseURL = "https://localhost:7240";

        public PlanetsController(ILogger<PlanetsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> GetAllPlanetsAsync()
        {
            List<Planets> planets = new List<Planets>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Planets/GetPlanets"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    planets = JsonConvert.DeserializeObject<Resultes<Planets>>(apiResult).Results;
                }

            }
            return View(planets);
        }
    }
}
