using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    [Table("App_Groups")]
    public class AppGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }
    }
}
