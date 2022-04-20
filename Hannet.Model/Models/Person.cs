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
    [Table("Persons")]
    public class Person : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        [Required]
        [MaxLength(255)]
        public string PersonName { get; set; }
        [Required]
        [MaxLength(255)]
        public string AliasID { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public int PlaceId { get; set; }
        public bool Type { get; set; }
        public Place Place { get; set; }
        public List<PersonImage> PersonImage { get; set; }
    }
}
