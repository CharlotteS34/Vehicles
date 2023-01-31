using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicles.Repository.Entities
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }
        [ForeignKey(nameof(Colour))]
        public int ColourId { get; set; }
        public Colour Colour { get; set; }
        [ForeignKey(nameof(Model))]
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public int Year { get; set; }
    }
}
