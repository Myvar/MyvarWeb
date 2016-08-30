using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Responce
{
    public class WebSharpResponce : HttpResponce
    {
        public string Content { get; set; }

        public WebSharpResponce(string content)
        {
            Content = content;
        }

        public override void Write(NetworkStream ns)
        {
            {
                var buf = Encoding.ASCII.GetBytes(Headers.ToString() + Environment.NewLine + Environment.NewLine);
                ns.Write(buf, 0, buf.Length);
            }
            {
                var buf = Encoding.ASCII.GetBytes(Content);
                ns.Write(buf, 0, buf.Length);
            }
        }
    }
}
