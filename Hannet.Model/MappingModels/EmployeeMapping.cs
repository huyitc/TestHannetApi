using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Model.MappingModels
{
    public class EmployeeMapping
    {
        public string EmployeeName { get; set; }
        public int EmployeeAge { get; set; }
        public string Sex { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
