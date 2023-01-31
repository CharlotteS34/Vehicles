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
            var vehicle = new VehicleDTO 
            {
                ModelId = request.ModelId,
                ColourId = request.ColourId,
                Year = request.Year
            };
            var vehicleResult = await _vehicleService.CreateVehicle(vehicle);
            return Created($"vehicle/{vehicleResult.VehicleId}", vehicleResult);
        }
    }
}