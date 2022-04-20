using Hannet.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("Places")]
    public class Place : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PlaceName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        public List<AppUser> AppUser { get; set; }
        public List<Device> Device { get; set; }
        public List<Person> Person { get; set; }
    }
}
