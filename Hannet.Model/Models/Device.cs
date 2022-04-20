using Hannet.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    [Table("Devices")]
    public class Device:Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string DeviceName { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
