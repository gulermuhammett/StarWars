using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models.DTOs;

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

        public async Task<IActionResult> GetAllPlanetCardAsync()
        {
            List<Planets> planets = new List<Planets>();
            List<PlanetDTO> planetDTOs = new List<PlanetDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm planetleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Planets/GetPlanets"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    planets = JsonConvert.DeserializeObject<Resultes<Planets>>(apiResult).Results;
                }

                //API den gelen tüm planetlerin ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var planet in planets)
                {
                    // Dosya ismiyle eşleşen karakter varsa, planetDTOs'yu oluştur.
                    //planetDTO'lara Planet sınıfından gerekli özellikleri ve Img adını ataması yapıldı (AUTOMapper kullanılabilir!!!)
                    string matchingFileName = files.FirstOrDefault(item => (planet.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    planetDTOs.Add(new PlanetDTO
                    {
                        Name=planet.Name,
                        Diameter=planet.Diameter,
                        Climate=planet.Climate,
                        Gravity=planet.Gravity,
                        Terrain=planet.Terrain,
                        Surface_water=planet.Surface_water,
                        Population=planet.Population,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(planetDTOs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPlanetCardAsync(string search)
        {
            List<Planets> planets = new List<Planets>();
            List<PlanetDTO> planetDTOs = new List<PlanetDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                if (search == null || search == "")
                {
                    //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetAllFilms"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        planets = JsonConvert.DeserializeObject<Resultes<Planets>>(apiResult).Results;
                    }
                }
                else
                {
                    //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetBySearchFilms?search={search}"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        planets = JsonConvert.DeserializeObject<List<Planets>>(apiResult);
                    }
                }

                //API den gelen tüm karakterlere ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var planet in planets)
                {

                    // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluşturacak
                    string matchingFileName = files.FirstOrDefault(item => (planet.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    planetDTOs.Add(new PlanetDTO
                    {
                        Name = planet.Name,
                        Diameter = planet.Diameter,
                        Climate = planet.Climate,
                        Gravity = planet.Gravity,
                        Terrain = planet.Terrain,
                        Surface_water = planet.Surface_water,
                        Population = planet.Population,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(planetDTOs);
        }
    }
}
