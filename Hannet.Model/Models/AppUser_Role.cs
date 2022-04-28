using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hannet.Model.Models
{
    public class AppUser_Role
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { set; get; }

        [Key]
        [Column(Order = 2)]
        public string RoleId { set; get; }
    }
}
