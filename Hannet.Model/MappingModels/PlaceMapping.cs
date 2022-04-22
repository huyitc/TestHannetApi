using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.MappingModels
{
    public class PlaceMapping
    {
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PlaceName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
}
