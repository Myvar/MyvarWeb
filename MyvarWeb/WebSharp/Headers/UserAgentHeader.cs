
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp;

namespace MyvarWeb.Headers
{
    public class UserAgentHeader : HttpField
    {
        public string Agent { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("User-Agent:");
        }

        public override HttpField Parse(string s)
        {
            var re = new UserAgentHeader();

            re.Agent = s.Remove(0, 10);

            return re;
        }

        public override string ToString()
        {
            return "User-Agent: " + Agent;
        }
    }
}
