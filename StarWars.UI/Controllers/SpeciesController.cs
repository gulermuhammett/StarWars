using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models.DTOs;

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
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Species/GetAllSpecies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Species>>(apiResult).Results;
                }

            }
            return View(species);
        }

        public async Task<IActionResult> GetAllSpeciesCardAsync()
        {
            List<Species> species = new List<Species>();
            List<SpeciesDTO> speciesDTOs = new List<SpeciesDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Species/GetAllSpecies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    species = JsonConvert.DeserializeObject<Resultes<Species>>(apiResult).Results;
                }

                //API den gelen tüm karakterlere ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var oneSpecies in species)
                {
                    // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluştur
                    string matchingFileName = files.FirstOrDefault(item => (oneSpecies.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));
                    
                        speciesDTOs.Add(new SpeciesDTO
                        {
                            Name = oneSpecies.Name,
                            Classification = oneSpecies.Classification,
                            Designation = oneSpecies.Designation,
                            Skin_colors = oneSpecies.Skin_colors,
                            Language = oneSpecies.Language,
                            Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                        });
                }
            }

            return View(speciesDTOs);
        }
    }
}
