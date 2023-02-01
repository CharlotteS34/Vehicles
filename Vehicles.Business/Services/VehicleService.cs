using Vehicles.Common.Models;
using Vehicles.Common.Models.Results;
using Vehicles.Common.Repositories;
using Vehicles.Common.Services;

namespace Vehicles.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ServiceResult<VehicleDetailsDTO>> CreateVehicle(VehicleDetailsDTO vehicleDetails)
        {
            var colour = await _vehicleRepository.GetColour(vehicleDetails.Colour);
            if (colour == null) 
            {
                return new ServiceResult<VehicleDetailsDTO>
                {
                    Message = $"Invalid colour {vehicleDetails.Colour}.",
                    Success = false
                };
            }
            var model = await _vehicleRepository.GetModel(vehicleDetails.Model, vehicleDetails.Make);
            if (model == null)
            {
                return new ServiceResult<VehicleDetailsDTO>
                {
                    Message = $"Invalid make/model {vehicleDetails.Make} {vehicleDetails.Model}.",
                    Success = false
                };
            }
            var vehicle = await _vehicleRepository.CreateVehicle(new VehicleDTO
            {
                Year = vehicleDetails.Year,
                ModelId = model.ModelId,
                ColourId = colour.ColourId
            });
            vehicleDetails.VehicleId = vehicle.VehicleId;
            return new ServiceResult<VehicleDetailsDTO>
            {
                Success = true,
                ResultValue = vehicleDetails
            };
        }

        public async Task DeleteVehicle(int vehicleId)
            => await _vehicleRepository.DeleteVehicle(vehicleId);

        public async Task<VehicleDetailsDTO?> GetVehicle(int vehicleId)
            => await _vehicleRepository.GetVehicleDetails(vehicleId);

        public async Task<IEnumerable<VehicleDetailsDTO>> GetVehicles()
            => await _vehicleRepository.GetVehicles();

        public async Task<ServiceResult> UpdateVehicle(VehicleDetailsDTO vehicleUpdate)
        {
            var vehicle = await _vehicleRepository.GetVehicle(vehicleUpdate.VehicleId.Value);
            if (vehicle == null) 
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Vehicle with id {vehicleUpdate.VehicleId} not found."
                };
            }
            vehicle.Year = vehicleUpdate.Year;
            if (vehicleUpdate.Colour != null) 
            {
                var colour = await _vehicleRepository.GetColour(vehicleUpdate.Colour);
                if (colour == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = $"Invalid colour {vehicleUpdate.Colour}."
                    };
                }
                else 
                {
                    vehicle.ColourId = colour.ColourId;
                }
            }
            if (vehicleUpdate.Make == null)
            {
                if (vehicleUpdate.Model != null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Invalid make/model."
                    };
                }
            }
            else 
            {
                if (vehicleUpdate.Model == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Invalid make/model."
                    };
                }
                else 
                {
                    var model = await _vehicleRepository.GetModel(vehicleUpdate.Model, vehicleUpdate.Make);
                    if (model == null) 
                    {
                        return new ServiceResult
                        {
                            Success = false,
                            Message = "Invalid make/model."
                        };
                    }
                    vehicle.ModelId = model.ModelId;
                }
            }
            await _vehicleRepository.UpdateVehicle(vehicle);
            return new ServiceResult
            {
                Success = true
            };
        }
    }
}
