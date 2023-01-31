using System.ComponentModel.DataAnnotations;

namespace Vehicles.Repository.Entities
{
    public class Make
    {
        [Key]
        public int MakeId { get; set; }
        public string MakeName { get; set; }
    }
}
