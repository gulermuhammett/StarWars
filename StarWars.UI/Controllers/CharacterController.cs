using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models.DTOs;

namespace StarWars.UI.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ILogger<CharacterController> _logger;
        string baseURL = "https://localhost:7240";

        public CharacterController(ILogger<CharacterController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> AllCharactersAsync()
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

        
        public async Task<IActionResult> GetAllCharacterCardAsync()
        {
            List<People> people = new List<People>();
            Planets planet = new Planets();
            List<CharacterDTO> characterDTOs = new List<CharacterDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Character/GetCharacters"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    people = JsonConvert.DeserializeObject<Resultes<People>>(apiResult).Results;
                }

                //API den gelen tüm karakterlere ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 
                
                foreach (var person in people)
                {
                    string[] segments = person.Homeworld.Split('/');
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Planets/GetByPlanetId?id={segments[segments.Length - 2]}"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        planet = JsonConvert.DeserializeObject<Planets>(apiResult);

                        // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluştur
                        string matchingFileName = files.FirstOrDefault(item => (person.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));
                        if (matchingFileName != null)
                        {
                            characterDTOs.Add(new CharacterDTO
                            {
                                Name = person.Name,
                                HomeWorld = planet.Name == null ? "Unknown" : planet.Name,
                                Img = matchingFileName
                            });
                        }
                    }
                }
            }

            return View(characterDTOs);
        }

    }
}
