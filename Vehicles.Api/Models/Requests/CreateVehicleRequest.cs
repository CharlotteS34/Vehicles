using System.ComponentModel.DataAnnotations;

namespace Vehicles.Api.Models.Requests
{
    public class CreateVehicleRequest
    {
        [Range(1900, Int32.MaxValue)]
        public int Year { get; set; }
        public int ColourId { get; set; }
        public int ModelId { get; set; }
    }
}
