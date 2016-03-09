using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class HttpRequest
    {
        public HttpHeader Header { get; set;  } 

        public HttpRequest(byte[] data)
        {
            Header = new HttpHeader(data);
        }
    }
}
