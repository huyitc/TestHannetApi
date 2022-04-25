using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("AppUser_Groups")]
    public class AppUser_Group
    {
        [StringLength(128)]
        [Key]
        [Column(Order = 1)]
        public string UserId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int GroupId { set; get; }

        /*[ForeignKey("UserId")]
        public virtual AppUser AppUser { set; get; }

        [ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { set; get; }*/
    }
}
