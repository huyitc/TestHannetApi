using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class EmployeeViewModels
    {
        public int EmployeeId { get; set; }
        [MaxLength(255)]
        public string EmployeeName { get; set; }
        public int EmployeeAge { get; set; }
        [MaxLength(5)]
        public string Sex { get; set; }
    }
}
