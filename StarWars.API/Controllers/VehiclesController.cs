using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarWars.API.Entities;
using StarWars.API.Services;

namespace StarWars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : Controller
    {
        private readonly GenericService<Vehicles> genericService;

        public VehiclesController(GenericService<Vehicles> vehiclesService)
        {
            genericService = vehiclesService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetVehicles()
        {
            try
            {
                var result = await genericService.GetAll();
                var data = JsonConvert.DeserializeObject<Resultes<Vehicles>>(result);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetByVehicleName(string name)
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
        public async Task<ActionResult> GetByVehicleId(int id)
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
        public async Task<ActionResult> GetBySearchVehicles(string search)
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
