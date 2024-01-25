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
        [HttpGet]
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

        [HttpGet]
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
                    // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluştur
                    string matchingFileName = files.FirstOrDefault(item => (person.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    characterDTOs.Add(new CharacterDTO
                    {
                        Name = person.Name,
                        Skin_color = person.Skin_color,
                        Eye_color = person.Eye_color,
                        Birth_year = person.Birth_year,
                        Gender = person.Gender,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(characterDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCharacterCardAsync(string search)
        {
            List<People> people = new List<People>();
            Planets planet = new Planets();
            List<CharacterDTO> characterDTOs = new List<CharacterDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                if (search == null || search == "")
                {
                    //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Character/GetCharacters"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        people = JsonConvert.DeserializeObject<Resultes<People>>(apiResult).Results;
                    }
                }
                else
                {
                    //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Character/GetBySearchCharacters?search={search}"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        people = JsonConvert.DeserializeObject<List<People>>(apiResult);
                    }
                }

                //API den gelen tüm karakterlere ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var person in people)
                {
                    
                    // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluşturacak
                    string matchingFileName = files.FirstOrDefault(item => (person.Name + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    characterDTOs.Add(new CharacterDTO
                    {
                        Name = person.Name,
                        Skin_color = person.Skin_color,
                        Eye_color = person.Eye_color,
                        Birth_year = person.Birth_year,
                        Gender = person.Gender,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(characterDTOs);
        }

    }
}
