using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class ServerHeader : HttpField
    {
        public string Server { get; set; } = "MyvarWeb/0.1.0";


        public override bool Id(string s)
        {
            throw new NotImplementedException();
        }

        public override HttpField Parse(string s)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Server: " + Server;
        }
    }
}
