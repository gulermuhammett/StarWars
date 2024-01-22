using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;

namespace StarWars.UI.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly ILogger<SpeciesController> _logger;
        string baseURL = "https://localhost:7240";

        public SpeciesController(ILogger<SpeciesController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> GetAllSpeciesAsync()
        {
            List<Species> species = new List<Species>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Species/GetSpecies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Species>>(apiResult).Results;
                }

            }
            return View(species);
        }
    }
}
