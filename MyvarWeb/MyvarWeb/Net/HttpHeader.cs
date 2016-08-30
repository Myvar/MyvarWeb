using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public class HttpHeader
    {
        private string _raw { get; set; }

        public HttpHeader(string raw)
        {
            _raw = raw;
        }

        public HttpHeader()
        {

        }

        public override string ToString()
        {
          return  "HTTP/1.1 200 Life is good" + Environment.NewLine +
            "Date: " + DateTime.Now + Environment.NewLine +
            "Server: " + "MyvarWeb/0.1" + Environment.NewLine +
            "Connection: " + "Closed";
        }
    }
}
