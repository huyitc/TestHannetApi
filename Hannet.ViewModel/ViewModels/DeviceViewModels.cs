using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.ViewModel.ViewModels
{
    public class DeviceViewModels
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int PlaceId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
