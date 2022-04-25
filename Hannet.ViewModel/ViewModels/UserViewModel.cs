using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public int? EM_ID { get; set; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string PhoneNumber { set; get; }
    }
}
