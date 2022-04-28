using System;
using System.Collections.Generic;

namespace Hannet.ViewModel.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public int? EmployeeId { get; set; }
        //public string Bio { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string PhoneNumber { set; get; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? TimeLogin { get; set; }
        public int? CountLogin { get; set; }

        public IEnumerable<AppGroupViewModel> Groups { set; get; }
    }
}
