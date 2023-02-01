using System.ComponentModel.DataAnnotations;

namespace Vehicles.Api.Models.Requests
{
    public class UpdateVehicleRequest
    {
        public int VehicleId { get; set; }
        [Range(1900, Int32.MaxValue)]
        public int Year { get; set; }
        public string? Colour { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
    }
}
