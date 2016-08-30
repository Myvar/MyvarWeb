using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class ConnectionHeader : HttpField
    {
        public string Connection { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("Connection:");
        }

        public override HttpField Parse(string s)
        {
            var re = new ConnectionHeader();

            re.Connection = s.Remove(0, 11);

            return re;
        }

        public override string ToString()
        {
            return "Connection: " + Connection;
        }
    }
}
