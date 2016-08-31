using MyvarWeb.Interpiters;
using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Responce
{
    public class WebSharpResponce : HttpResponce
    {
        public string Content { get; set; }

        private HttpRequest req { get; set; }

        public WebSharpResponce(string content, HttpRequest r)
        {
            Content = content;
            req = r;
        }

        public override void Write(NetworkStream ns)
        {
            var s = File.ReadAllText(Content);
            var ws = new WebSharpInterpiter(s, Headers, req );
            var cont = ws.Gencontent();
            
            {
                var hds = Headers.ToString();
                if (hds != "")
                {
                    var buf = Encoding.ASCII.GetBytes(hds + Environment.NewLine);
                    ns.Write(buf, 0, buf.Length);
                }
            }
            {
                var hds = ws.Headers.ToString();
                if (hds != "")
                {
                    var buf = Encoding.ASCII.GetBytes(hds + Environment.NewLine + Environment.NewLine);
                    ns.Write(buf, 0, buf.Length);
                }
            }
            {
                
                
                var buf = Encoding.ASCII.GetBytes(cont);
                ns.Write(buf, 0, buf.Length);
            }
        }
    }
}
