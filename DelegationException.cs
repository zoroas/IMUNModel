using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalMUNManager.model
{
    public class DelegationException:Exception
    {
        public DelegationException(String msg) :base(msg)
        {
        }
    }
}
