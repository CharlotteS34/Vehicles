using Vehicles.Common.Models;
using Vehicles.Common.Models.Results;

namespace Vehicles.Common.Services
{
    public interface IVehicleService
    {
        public Task<IEnumerable<VehicleDetailsDTO>> GetVehicles();
        public Task<VehicleDetailsDTO?> GetVehicle(int vehicleId);
        public Task<ServiceResult<VehicleDetailsDTO>> CreateVehicle(VehicleDetailsDTO vehicleDetails);
        public Task DeleteVehicle(int vehicleId);
        public Task<ServiceResult> UpdateVehicle(VehicleDetailsDTO vehicleUpdate);
    }
}
