using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;

namespace StarWars.UI.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ILogger<VehiclesController> _logger;
        string baseURL = "https://localhost:7240";

        public VehiclesController(ILogger<VehiclesController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            List<Vehicles> species = new List<Vehicles>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Vehicles/GetVehicles"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Vehicles>>(apiResult).Results;
                }

            }
            return View(species);
        }
    }
}
