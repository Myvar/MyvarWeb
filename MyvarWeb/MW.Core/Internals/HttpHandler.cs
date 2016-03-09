using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class HttpHandler
    {
        private TcpListener _tcp { get; set; }

        public int Port = 80;

        public HttpHandler(int port)
        {
            Port = port;
            _tcp = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _tcp.Start();

            ThreadPool.QueueUserWorkItem((xyz) =>
            {
                while (true)
                {
                    HandleClient(_tcp.AcceptTcpClient());
                }
            });
        }


        private void HandleClient(TcpClient c)
        {
            ThreadPool.QueueUserWorkItem((xyz) =>
            {

                var ns = c.GetStream();

                while (c.Connected)
                {
                    if (ns.DataAvailable)
                    {
                        byte[] buffer = new byte[4096];
                        int bytesread = ns.Read(buffer, 0, buffer.Length);
                        Array.Resize(ref buffer, bytesread);

                        var req = new HttpRequest(buffer);

                        var res = Globals.cfg.VhostList.Where((x) => { return Regex.IsMatch(req.Header["Host"].Value, x.host) && x.port == Port; }).First().HandleRequest(req);

                        if (res.FlushOnClose)
                        {
                            if (!res.ConnectionOpen)
                            {
                                var sendbuffer = res.Flush();
                                ns.Write(sendbuffer, 0, sendbuffer.Length);
                            }
                        }
                        else
                        {
                            var sendbuffer = res.Flush();
                            ns.Write(sendbuffer, 0, sendbuffer.Length);
                        }

                        if (!res.ConnectionOpen)
                        {
                            ns.Flush();
                            ns.Close();
                            ns.Dispose();
                            c.Close();
                        }

                    }
                }
            });
        }
    }
}
