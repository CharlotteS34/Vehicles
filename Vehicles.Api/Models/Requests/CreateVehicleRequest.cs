using System.ComponentModel.DataAnnotations;

namespace Vehicles.Api.Models.Requests
{
    public class CreateVehicleRequest
    {
        [Range(1900, Int32.MaxValue)]
        public int Year { get; set; }
        [Required]
        public string Colour { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
    }
}
