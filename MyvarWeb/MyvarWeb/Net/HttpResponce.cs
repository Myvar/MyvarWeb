using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public abstract class HttpResponce
    {
        public HttpHeader Headers { get; set; } = new HttpHeader();

        public abstract void Write(NetworkStream ns);
    }
}
