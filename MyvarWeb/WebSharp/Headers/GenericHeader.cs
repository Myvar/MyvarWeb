
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp;

namespace MyvarWeb.Headers
{
    public class GenericHeader : HttpField
    {
        public string Raw { get; set; }

        public override bool Id(string s)
        {
            return true;
        }

        public override HttpField Parse(string s)
        {
            var re = new GenericHeader();

            re.Raw = s;

            return re;
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}
