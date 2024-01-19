using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.API.Services;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : Controller
    {
        private readonly GenericService<Films> genericService;

        public FilmsController(GenericService<Films> characterService)
        {
            genericService = characterService;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetAllFilms()
        {
            try
            {
                var result = await genericService.GetAll();
                var data = JsonConvert.DeserializeObject<Resultes<Films>>(result);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetByFilmTitle(string name)
        {
            try
            {
                var result = await genericService.GetDefault(x => x.Title.ToLower().Contains(name.ToLower()));
                if (result.Any()) // Liste boşsa NotFound döndürecek.
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"Character with name {name} not found.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetByFilmId(int id)
        {
            try
            {
                var result = await genericService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
