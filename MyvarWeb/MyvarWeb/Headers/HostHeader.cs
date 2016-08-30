using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class HostHeader : HttpField
    {
        public string Host { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("Host:");
        }

        public override HttpField Parse(string s)
        {
            var re = new HostHeader();

            re.Host = s.Remove(0, 5);

            return re;
        }

        public override string ToString()
        {
            return "Host: " + Host;
        }
    }
}
