
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Headers
{
    public class AcceptEncodingHeader : HttpField
    {
        public List<string> AcceptedEncodings { get; set; } = new List<string>();

        public override bool Id(string s)
        {
            return s.StartsWith("Accept-Encoding:");
        }

        public override HttpField Parse(string s)
        {
            var re = new AcceptEncodingHeader();

            foreach (var i in s.Remove(0, 16).Split(','))
            {
                re.AcceptedEncodings.Add(i);
            }

            return re;
        }
    }
}
