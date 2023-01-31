using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicles.Repository.Entities
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }
        [Required]
        public string ModelName { get; set; }
        [ForeignKey(nameof(Make))]
        public int MakeId { get; set; }
        public Make Make { get; set; }
    }
}
