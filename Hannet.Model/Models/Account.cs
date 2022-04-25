using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.Models
{
    [Table("Accounts")]
    public partial class Account
    {
        [Key]
        public int AccId { get; set; }

        public int? EM_ID { get; set; }

        [StringLength(30)]
        public string Acc_Username { get; set; }

        [StringLength(200)]
        public string Acc_Password { get; set; }

        public bool? Acc_Status { get; set; }

        [StringLength(50)]
        public string Shift { get; set; }

        public TimeSpan? From_Time { get; set; }

        public TimeSpan? To_Time { get; set; }

        [StringLength(400)]
        public string IP_Login { get; set; }

        [StringLength(200)]
        public string Area { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
