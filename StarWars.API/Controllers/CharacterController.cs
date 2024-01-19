using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.API.Services;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : Controller
    {
        private readonly GenericService<People> genericService;


        public CharacterController(GenericService<People> characterService )
        {
            genericService = characterService;


        }
       

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetCharacters()
        {
            try
            {
                var result = await genericService.GetAll();
                var data = JsonConvert.DeserializeObject<Resultes<People>>(result);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetByCharacterName(string name)
        {
            try
            {
                var result = await genericService.GetDefault(x=>x.Name.ToLower().Contains(name.ToLower()));
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
        public async Task<ActionResult> GetByCharacterId(int id)
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
