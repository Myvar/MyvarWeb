using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class AcceptLanguageHeader : HttpField
    {
        public List<string> AcceptedLanguages { get; set; } = new List<string>();

        public override bool Id(string s)
        {
            return s.StartsWith("Accept-Language:");
        }

        public override HttpField Parse(string s)
        {
            var re = new AcceptLanguageHeader();

            foreach (var i in s.Remove(0, 16).Split(','))
            {
                re.AcceptedLanguages.Add(i);
            }

            return re;
        }
    }
}
