using Vehicles.Common.Models;

namespace Vehicles.Common.Repositories
{
    public interface IVehicleRepository
    {
        public Task<IEnumerable<VehicleDetailsDTO>> GetVehicles();
        public Task<VehicleDetailsDTO?> GetVehicle(int vehicleId);
        public Task<VehicleDTO> CreateVehicle(VehicleDTO vehicle);
        public Task DeleteVehicle(int vehicleId);
    }
}
