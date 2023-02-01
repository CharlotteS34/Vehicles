using Vehicles.Common.Models;

namespace Vehicles.Common.Repositories
{
    public interface IVehicleRepository
    {
        public Task<IEnumerable<VehicleDetailsDTO>> GetVehicles();
        public Task<VehicleDetailsDTO?> GetVehicleDetails(int vehicleId);
        public Task<VehicleDTO?> GetVehicle(int vehicleId);
        public Task<VehicleDTO> CreateVehicle(VehicleDTO vehicle);
        public Task DeleteVehicle(int vehicleId);
        public Task<ColourDTO?> GetColour(string colourName);
        public Task<ModelDTO?> GetModel(string modelName, string makeName);
        public Task UpdateVehicle(VehicleDTO vehicle);
    }
}
