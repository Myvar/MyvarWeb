using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Headers
{
    public class CookieHeader : HttpField
    {
        public string Cookies { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("Cookie:");
        }

        public override HttpField Parse(string s)
        {
            var re = new CookieHeader();

            re.Cookies = s.Remove(0, 7);

            return re;
        }

        public override string ToString()
        {
            return "Set-Cookie: " + Cookies;
        }
    }
}
