using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models.DTOs;

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

        public async Task<IActionResult> GetAllVehicleCardAsync()
        {
            List<Vehicles> vehicles = new List<Vehicles>();
            List<VehicleDTO> vehicleDTO = new List<VehicleDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm filmleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Starships/GetAllStarships"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    vehicles = JsonConvert.DeserializeObject<Resultes<Vehicles>>(apiResult).Results;
                }

                //API den gelen tüm filmlerin ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var vehicle in vehicles)
                {
                    // Dosya ismiyle eşleşen karakter varsa, filmDTOs'yu oluştur.
                    //filmDTO'lara Film sınıfından gerekli özellikleri ve Img adını ata (AUTOMapper kullanılabilir!!!)
                    string matchingFileName = files.FirstOrDefault(item => (vehicle.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    vehicleDTO.Add(new VehicleDTO
                    {
                        Name = vehicle.Name,
                        Model = vehicle.Model,
                        Manufacturer = vehicle.Manufacturer,
                        Cost_in_credits = vehicle.Cost_in_credits,
                        Passengers = vehicle.Passengers,
                        Crew=vehicle.Crew,
                        Cargo_capacity = vehicle.Cargo_capacity,
                        Vehicle_class = vehicle.Vehicle_class,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(vehicleDTO);
        }
    }
}
