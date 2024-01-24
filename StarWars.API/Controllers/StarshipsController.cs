using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.API.Services;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipsController : Controller
    {
        private readonly GenericService<Starships> genericService;

        public StarshipsController(GenericService<Starships> characterService)
        {
            genericService = characterService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetAllStarships()
        {
            try
            {
                var result = await genericService.GetAll();
                var data = JsonConvert.DeserializeObject<Resultes<Starships>>(result);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetByStarshipsName(string name)
        {
            try
            {
                var result = await genericService.GetDefault(x => x.Name.ToLower().Contains(name.ToLower()));
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
        public async Task<ActionResult> GetByStarshipId(int id)
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
