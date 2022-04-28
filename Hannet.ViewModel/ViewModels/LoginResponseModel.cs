using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class LoginResponseModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
    }
}
