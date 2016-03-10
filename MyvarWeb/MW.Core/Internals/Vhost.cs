using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class Vhost
    {
        [JsonIgnore]
        public Func<HttpRequest, string, string, Vhost, HttpResponce> HandleRequest { get; set; } = (x, w, h, v) =>
        {
            return Serve.Serveuri(x, w, h, v);
        };

        public string host { get; set; }
        public int port { get; set; }
        public string www { get; set; } = "";
        public List<string> Cgi { get; set; } = new List<string>();

        public Vhost()
        {

        }

        public Vhost(string ahost)
        {
            host = ahost.Replace(ahost.Split(':').Last(), "").TrimEnd(':');
            port = Convert.ToInt32(ahost.Split(':').Last());

            
        }
    }
}
