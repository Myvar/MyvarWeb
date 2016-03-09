using MW.Core.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core
{
    public class Engine
    {
        public void Start()
        {
            var webserver = new Http();
            webserver.Start();
            while (true) ; // dont kill main thread
        }
    }
}
