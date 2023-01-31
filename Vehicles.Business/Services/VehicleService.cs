using Vehicles.Common.Models;
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

        public async Task<VehicleDTO> CreateVehicle(VehicleDTO vehicle)
            => await _vehicleRepository.CreateVehicle(vehicle);   

        public async Task DeleteVehicle(int vehicleId)
            => await _vehicleRepository.DeleteVehicle(vehicleId);

        public async Task<VehicleDetailsDTO?> GetVehicle(int vehicleId)
            => await _vehicleRepository.GetVehicle(vehicleId);

        public async Task<IEnumerable<VehicleDetailsDTO>> GetVehicles()
            => await _vehicleRepository.GetVehicles();
    }
}
