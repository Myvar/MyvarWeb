
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Headers
{
    public class AcceptHeader : HttpField
    {
        public List<string> AcceptedTypes { get; set; } = new List<string>();

        public override bool Id(string s)
        {
            return s.StartsWith("Accept:");
        }

        public override HttpField Parse(string s)
        {
            var re = new AcceptHeader();

            foreach(var i in s.Remove(0, 7).Split(','))
            {
                re.AcceptedTypes.Add(i);
            }

            return re;
        }
    }
}
