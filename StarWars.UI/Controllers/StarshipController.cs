using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models.DTOs;

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
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Starships/GetAllStarships"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Starships>>(apiResult).Results;
                }

            }
            return View(species);
        }

        public async Task<IActionResult> GetAllStarshipCardAsync()
        {
            List<Starships> starships = new List<Starships>();
            List<StarshipDTO> filmDTOs = new List<StarshipDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm filmleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Starships/GetAllStarships"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    starships = JsonConvert.DeserializeObject<Resultes<Starships>>(apiResult).Results;
                }

                //API den gelen tüm filmlerin ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var starship in starships)
                {
                    // Dosya ismiyle eşleşen karakter varsa, filmDTOs'yu oluştur.
                    //filmDTO'lara Film sınıfından gerekli özellikleri ve Img adını ata (AUTOMapper kullanılabilir!!!)
                    string matchingFileName = files.FirstOrDefault(item => (starship.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    filmDTOs.Add(new StarshipDTO
                    {
                        Name = starship.Name,
                        Model = starship.Model,
                        Manufacturer = starship.Manufacturer,
                        Cost_in_credits = starship.Cost_in_credits,
                        Passengers = starship.Passengers,
                        Cargo_capacity= starship.Cargo_capacity,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(filmDTOs);
        }
    }
}
