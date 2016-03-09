using MA.Core.Util;
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
            Logger.Log("Http server starting");
        }

        public void Start()
        {
            List<int> Ports = new List<int>();
            Logger.Log("Starting vhosts");
            foreach (var i in Globals.cfg.VhostList)
            {
                Logger.Log("Found vhost: " + i.host + ":" + i.port);
                if (!Ports.Contains(i.port))
                {
                    Ports.Add(i.port);
                }
            }

            foreach(var i in Ports)
            {
                var handle = new HttpHandler(i);
                handle.Start();
                Logger.Log("Binding to port " + i);
                Handlers.Add(handle);
            }

            foreach (var i in Globals.cfg.ProxyList)
            {
                Logger.Log("Starting Porexy on port " + i.Aport + " to " + i.Bhost + ":" + i.Bport);
                Logger.Log("Binding to port " + i.Aport);
                i.Start();
            }
            
        }
    }
}
