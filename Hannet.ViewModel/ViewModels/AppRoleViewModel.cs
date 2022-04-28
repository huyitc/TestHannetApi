using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class AppRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Discriminator { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string ParentId { get; set; }
    }
}
