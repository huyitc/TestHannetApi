using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class AppGroupViewModel
    {
        public int ID { set; get; }

        public string NAME { set; get; }

        public string DESCRIPTION { set; get; }

        public IEnumerable<AppRoleViewModel> Roles { get; set; }
    }
}
