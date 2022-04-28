using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {

        }
        [StringLength(128)]
        public string ParentId { get; set; }


        public DateTime? CreatedDate { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        /*[ForeignKey("GroupId")]
        public virtual AppGroup AppGroup { get; set; }*/
    }
}
