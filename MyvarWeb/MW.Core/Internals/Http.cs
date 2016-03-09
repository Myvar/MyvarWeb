using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class Http
    {
        public List<HttpHandler> Handlers { get; set; } = new List<HttpHandler>();

        public Http()
        {

        }

        public void Start()
        {
            List<int> Ports = new List<int>();

            foreach (var i in Globals.cfg.VhostList)
            {
                if (!Ports.Contains(i.port))
                {
                    Ports.Add(i.port);
                }
            }

            foreach(var i in Ports)
            {
                var handle = new HttpHandler(i);
                handle.Start();

                Handlers.Add(handle);
            }

            foreach (var i in Globals.cfg.ProxyList)
            {
                i.Start();
            }

        }
    }
}
