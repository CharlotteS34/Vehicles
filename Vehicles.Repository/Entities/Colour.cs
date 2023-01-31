using System.ComponentModel.DataAnnotations;

namespace Vehicles.Repository.Entities
{
    public class Colour
    {
        [Key]
        public int ColourId { get; set; }
        public string ColourName { get; set; }
    }
}
