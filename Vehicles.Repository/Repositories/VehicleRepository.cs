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

        public async Task DeleteVehicle(int vehicleId)
        {
            var vehicle = await _db.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _db.Vehicles.Remove(vehicle);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<ColourDTO?> GetColour(string colourName)
        {
            var colour = await _db.Colours.FirstOrDefaultAsync(x => x.ColourName == colourName);
            return colour == null ? null : new ColourDTO 
            {
                ColourId = colour.ColourId,
                ColourName = colour.ColourName
            };
        }

        public async Task<ModelDTO?> GetModel(string modelName, string makeName)
        {
            var model = await _db.Models
                .Include(x => x.Make)
                .FirstOrDefaultAsync(x => x.ModelName == modelName && x.Make.MakeName == makeName);
            return model == null ? null : new ModelDTO
            {
                ModelId= model.ModelId,
            };
        }

        public async Task<VehicleDTO?> GetVehicle(int vehicleId)
        {
            var vehicle = await _db.Vehicles.FindAsync(vehicleId);
            return vehicle == null ? null : new VehicleDTO 
            {
                VehicleId = vehicle.VehicleId,
                ColourId = vehicle.ColourId,
                ModelId = vehicle.ModelId,
                Year = vehicle.Year
            };
        }

        public async Task<VehicleDetailsDTO?> GetVehicleDetails(int vehicleId)
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

        public async Task UpdateVehicle(VehicleDTO vehicleUpdate)
        {
            var vehicle = await _db.Vehicles.FindAsync(vehicleUpdate.VehicleId.Value);
            if (vehicle == null) 
            {
                return;
            }
            vehicle.Year = vehicleUpdate.Year;
            vehicle.ColourId = vehicleUpdate.ColourId;
            vehicle.ModelId = vehicleUpdate.ModelId;
            _db.Vehicles.Attach(vehicle);
            await _db.SaveChangesAsync();
        }
    }
}
