
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp;

namespace MyvarWeb.Headers
{
    public class CacheControlHeader : HttpField
    {
        public string MaxAge { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("Cache-Control:");
        }

        public override HttpField Parse(string s)
        {
            var re = new CacheControlHeader();

            re.MaxAge = s.Remove(0, 14);

            return re;
        }
    }
}
