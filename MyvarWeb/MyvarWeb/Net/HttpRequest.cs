using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public class HttpRequest
    {
        private TcpClient _tcp = new TcpClient();

        public HttpHeader Header { get; set; }
        public string Body { get; set; }

        public HttpRequest(TcpClient tcp)
        {
            _tcp = tcp;
        }

        public void Parse()
        {
            var s = _tcp.GetStream();

            bool done = false;

            var sb = new StringBuilder();

            bool header = false;

            while(!done)
            {
                if(s.DataAvailable)
                {
                    sb.Append(Encoding.ASCII.GetString(new byte[] { (byte)s.ReadByte() }));
                    if (!header)
                    {
                        if (sb.ToString().EndsWith("\r\n\r\n"))
                        {
                            var headers = sb.ToString();
                            Header = new HttpHeader(headers);
                            header = true;
                            sb.Clear();
                        }
                    }
                }
                else
                {
                    Body = sb.ToString();
                    done = true;
                }
            }

            

        }
    }
}
