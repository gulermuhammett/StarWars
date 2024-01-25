using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.API.Services;
using System.Xml.Linq;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : Controller
    {
        private readonly GenericService<Species> genericService;

        public SpeciesController(GenericService<Species> characterService)
        {
            genericService = characterService;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetAllSpecies()
        {
            try
            {
                var result = await genericService.GetAll();
                var data = JsonConvert.DeserializeObject<Resultes<Species>>(result);
                // Liste boşsa NotFound döndürecek.
                if (result.Any()) return Ok(data);

                else return NotFound($"Character is not found.");
                
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetBySpeciesName(string name)
        {
            try
            {
                var result = await genericService.GetDefault(x => x.Name.ToLower().Contains(name.ToLower()));
                // Liste boşsa NotFound döndürecek.
                if (result.Any()) return Ok(result);

                else return NotFound($"Character with name {name} not found.");

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetBySpeciesId(int id)
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

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetBySearchSpecies(string search)
        {
            try
            {
                var result = await genericService.GetBySearch(search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
