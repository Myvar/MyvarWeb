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
        public Func<HttpRequest, HttpResponce> HandleRequest { get; set; } = (x) =>
        {
            return new HttpResponce("404 Page not found");
        };

        public string host { get; set; }
        public int port { get; set; }
        public string Handler { get; set; } = "";

        public Vhost(string ahost)
        {
            host = ahost.Replace(ahost.Split(':').Last(), "").TrimEnd(':');
            port = Convert.ToInt32(ahost.Split(':').Last());

            if (string.IsNullOrEmpty(Handler))
            {
                //bind HandleRequest to cgi stuff
            }
        }
    }
}
