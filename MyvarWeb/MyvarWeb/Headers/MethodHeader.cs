using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class MethodHeader : HttpField
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ProticallVertion { get; set; }

        public override bool Id(string s)
        {
            return s.StartsWith("GET ") || s.StartsWith("Post ");
        }

        public override HttpField Parse(string s)
        {
            var re = new MethodHeader();

            var segs = s.Split(' ');

            re.Method = segs[0];
            re.Path = segs[1];
            re.ProticallVertion = segs[2];

            return re;
        }

        public override string ToString()
        {
            return Method;
        }
    }
}
