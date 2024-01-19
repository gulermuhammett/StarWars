using Newtonsoft.Json;
using StarWars.API.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace StarWars.API.Services
{
    public class GenericService<T> where T : class
    {

        private readonly HttpClient _httpClient;
        string apiUrl = "https://swapi.dev/api" + $"/{typeof(T).Name.ToLower()}/";

        public GenericService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAll()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl );
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); ;
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }

        public async Task<List<T>> GetDefault(Expression<Func<T, bool>> exp)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Resultes<T>>(jsonString);

                // LINQ sorgusu ile exp koşulunu sağlayan öğeleri filtrele
                var filteredResults = data.Results.Where(exp.Compile()).ToList();

                return filteredResults;
            }
            else
            {
                // Hata durumunda bir hata mesajı içeren liste döndürecek.
                return new List<T> { (T)(object)response.StatusCode };
            }
        }

        public async Task<T> GetById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl+$"{id}/");
            

            if (response.IsSuccessStatusCode)
            {
                var jsonString= await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(jsonString);
                return data;
            }
            else
            {
                return (T)(object)response.StatusCode;
            }
        }


    }
}
