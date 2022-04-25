using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("AppRole_Groups")]
    public class AppRole_Group
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        [Column(Order = 2)]
        [StringLength(128)]
        [Key]
        public string RoleId { set; get; }

       /* [ForeignKey("RoleId")]
        public virtual AppRole AppRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { set; get; }*/
    }
}
