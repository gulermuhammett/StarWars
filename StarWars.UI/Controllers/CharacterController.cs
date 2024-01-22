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
            string dizin = "C:\\Users\\muham\\OneDrive\\Masaüstü\\template\\img";

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string imgFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "img");
            string[] path = Directory.GetFiles(dizin);
            List<string> files = new List<string>();
            foreach (string pathItem in path) {
                files.Add(pathItem.Split("\\")[pathItem.Split("\\").Length-1]);
            }
            
            if (!Directory.Exists(imgFolderPath))
            {
                Console.WriteLine("Dizin bulunamadı, oluşturuluyor...");
                Directory.CreateDirectory(imgFolderPath);
            }

            // wwwroot/img klasöründeki dosya isimlerini alıyoruz
            string[] imgFileNames = Directory.GetFiles(imgFolderPath, "*.jpg")
                                             .Select(Path.GetFileNameWithoutExtension)
                                             .ToArray();

            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Character/GetCharacters"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    people = JsonConvert.DeserializeObject<Resultes<People>>(apiResult).Results;
                }
                foreach (var person in people)
                {
                    string[] segments = person.Homeworld.Split('/');
                    string matchingFileName = imgFileNames.FirstOrDefault(imgFileName => imgFileName.Equals(person.Name, StringComparison.OrdinalIgnoreCase));
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Planets/GetByPlanetId?id={segments[segments.Length - 2]}"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        planet = JsonConvert.DeserializeObject<Planets>(apiResult);
                        foreach (var item in files)
                        {
                            if ((person.Name+".jpg")==item)
                            {
                                characterDTOs.Add(new CharacterDTO { Name = person.Name, HomeWorld = planet.Name == null ? "Unknown" : planet.Name, Img = item == null ? "StarWars.jpg" : item });
                            }
                        }
                        
                    }
                }
            }

            return View(characterDTOs);
        }
    }
}
