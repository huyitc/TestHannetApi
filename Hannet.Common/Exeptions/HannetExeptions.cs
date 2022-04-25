using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Common.Exeptions
{
    public class HannetExeptions : Exception
    {
        public HannetExeptions() 
        { 
        }
        public HannetExeptions(string message)
      : base(message)
        {
        }

        public HannetExeptions(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
