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
    [Table("AppUsers")]
    public class AppUser : IdentityUser
    {
        public int? EmployeeId { get; set; }
        public int? CountLogin { set; get; }
        public DateTime? TimeLogin { set; get; }
        public DateTime? CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<AppUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
