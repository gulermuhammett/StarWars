using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.UI.Models;
using StarWars.UI.Models.DTOs;
using System;
using System.Diagnostics;
using System.IO;

namespace StarWars.UI.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ILogger<FilmsController> _logger;
        string baseURL = "https://localhost:7240";

        public FilmsController(ILogger<FilmsController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Films> films = new List<Films>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetAllFilms"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    films = JsonConvert.DeserializeObject<Resultes<Films>>(apiResult).Results;
                }

            }
            return View(films);
        }


        public async Task<IActionResult> GetAllFilmCardAsync()
        {
            List<Films> films = new List<Films>();
            List<FilmDTO> filmDTOs = new List<FilmDTO>();

            // wwwroot/img klasöründeki dosya isimlerini almak için kullanıyoruz
            string path = Path.Combine("wwwroot", "img");
            string[] files = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();

            using (var httpClient = new HttpClient())
            {
                //API den tüm filmleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetAllFilms"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    films = JsonConvert.DeserializeObject<Resultes<Films>>(apiResult).Results;
                }

                //API den gelen tüm filmlerin ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var film in films)
                {
                    // Dosya ismiyle eşleşen karakter varsa, filmDTOs'yu oluştur.
                    //filmDTO'lara Film sınıfından gerekli özellikleri ve Img adını ata (AUTOMapper kullanılabilir!!!)
                    string matchingFileName = files.FirstOrDefault(item => (film.Title + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    filmDTOs.Add(new FilmDTO
                    {
                        Title = film.Title,
                        Opening_crawl = film.Opening_crawl,
                        Director = film.Director,
                        Producer = film.Producer,
                        Release_date = film.Release_date,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(filmDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllFilmCardAsync(string search)
        {
            List<Films> films = new List<Films>();
            List<FilmDTO> filmDTOs = new List<FilmDTO>();

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
                        films = JsonConvert.DeserializeObject<Resultes<Films>>(apiResult).Results;
                    }
                }
                else
                {
                    //API den tüm karakterleri almak için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Films/GetBySearchFilms?search={search}"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        films = JsonConvert.DeserializeObject<List<Films>>(apiResult);
                    }
                }

                //API den gelen tüm karakterlere ait gezegenler için atılan sorgu ve json verisinin obje listesine dönüşümünü yapıyoruz. 

                foreach (var film in films)
                {

                    // Dosya ismiyle eşleşen karakter varsa, characterDTOs'yu oluşturacak
                    string matchingFileName = files.FirstOrDefault(item => (film.Title + ".jpg").Equals(item, StringComparison.OrdinalIgnoreCase));

                    filmDTOs.Add(new FilmDTO
                    {
                        Title = film.Title,
                        Opening_crawl = film.Opening_crawl,
                        Director = film.Director,
                        Producer = film.Producer,
                        Release_date = film.Release_date,
                        Img = matchingFileName == null ? "StarWars.jpg" : matchingFileName
                    });
                }
            }

            return View(filmDTOs);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

