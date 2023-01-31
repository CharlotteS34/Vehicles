using Microsoft.EntityFrameworkCore;
using Vehicles.Common.Models;
using Vehicles.Common.Repositories;
using Vehicles.Repository.Entities;

namespace Vehicles.Repository.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _db;

        public VehicleRepository(VehicleDbContext db)
        {
            _db = db;
        }

        public async Task<VehicleDTO> CreateVehicle(VehicleDTO vehicle)
        {
            var newVehicle = new Vehicle 
            {
                ModelId = vehicle.ModelId,
                ColourId = vehicle.ColourId,
                Year = vehicle.Year
            };
            newVehicle = (await _db.Vehicles.AddAsync(newVehicle)).Entity;
            await _db.SaveChangesAsync();
            vehicle.VehicleId = newVehicle.VehicleId;
            return vehicle;
        }

        public Task DeleteVehicle(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleDetailsDTO?> GetVehicle(int vehicleId)
           => await _db.Vehicles
            .Include(x => x.Model)
            .ThenInclude(x => x.Make)
            .Include(x => x.Colour)
            .Where(x => x.VehicleId == vehicleId)
            .Select(x => new VehicleDetailsDTO
            {
                VehicleId = x.VehicleId,
                Make = x.Model.Make.MakeName,
                Model = x.Model.ModelName,
                Colour = x.Colour.ColourName,
                Year = x.Year
            })
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<VehicleDetailsDTO>> GetVehicles()
            => await _db.Vehicles
            .Include(x => x.Model)
            .ThenInclude(x => x.Make)
            .Include(x => x.Colour)
            .Select(x => new VehicleDetailsDTO 
            {
                VehicleId = x.VehicleId,
                Make = x.Model.Make.MakeName,
                Model = x.Model.ModelName,
                Colour = x.Colour.ColourName,
                Year = x.Year
            })
            .ToArrayAsync();

    }
}
