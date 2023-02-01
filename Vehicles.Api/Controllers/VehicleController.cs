using Microsoft.AspNetCore.Mvc;
using Vehicles.Api.Models.Requests;
using Vehicles.Common.Models;
using Vehicles.Common.Services;

namespace Vehicles.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {

        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IEnumerable<VehicleDetailsDTO>> GetAll()
            => await _vehicleService.GetVehicles();

        [HttpGet]
        [Route("{id?}")]
        public async Task<ActionResult<VehicleDetailsDTO>> Get(int id)
        {
            var vehicle = await _vehicleService.GetVehicle(id);
            if (vehicle == null)
            {
                return NotFound($"Vehicle with id {id} could not be found.");
            }
            else
            {
                return Ok(vehicle);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateVehicleRequest request) 
        {
            var vehicle = new VehicleDetailsDTO 
            {
                Make = request.Make,
                Model = request.Model,
                Colour = request.Colour,
                Year = request.Year
            };
            var result = await _vehicleService.CreateVehicle(vehicle);
            if (result.Success)
            {
                return Created($"vehicle/{result.ResultValue.VehicleId}", result.ResultValue);
            }
            else 
            {
                return BadRequest(result.Message);
            }
        }

        [HttpDelete]
        [Route("{id?}")]
        public async Task<ActionResult> Delete(int vehicleId) 
        {
            await _vehicleService.DeleteVehicle(vehicleId);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put(UpdateVehicleRequest request) 
        {
            var result = await _vehicleService.UpdateVehicle(new VehicleDetailsDTO 
            {
                VehicleId = request.VehicleId,
                Year = request.Year,
                Make = request.Make,
                Model = request.Model,
                Colour = request.Colour
            });
            if (result.Success) 
            {
                return Ok();
            }
            else
            {
                if (result.Message.ToUpper().Contains("NOT FOUND"))
                {
                    return NotFound(result.Message);
                }
                else 
                {
                    return BadRequest(result.Message);
                }
            }
        }
    }
}