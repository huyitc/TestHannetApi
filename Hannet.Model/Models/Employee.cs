using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("Employees")]
    public class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string EmployeeName { get; set; }
        public int EmployeeAge { get; set; }
        
        [Required, MaxLength(5)]
        public string Sex { get; set; }
        public string Image { get; set; }
        public AppUser AppUser { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
