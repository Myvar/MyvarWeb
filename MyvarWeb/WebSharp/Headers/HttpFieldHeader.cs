
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp;

namespace MyvarWeb.Headers
{
    public class HttpFieldHeader : HttpField
    {
        public string HttpVertion { get; set; } = "HTTP/1.1";
        public int Status { get; set; } = 200;
        public string Message { get; set; } = "Philippians 4:4 - be happy";

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
            return HttpVertion + " " + Status + " " + Message;
        }
    }
}
