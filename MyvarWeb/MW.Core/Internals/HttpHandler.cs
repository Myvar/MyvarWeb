using System;
using System.Collections.Generic;
using System.IO;
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
                    var cl = _tcp.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem((ff) =>{
                

                        HandleClient(cl);

                    });

                    Thread.Sleep(1);
                }
            });
        }


        private void HandleClient(TcpClient c)
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

                        var resmeth = Globals.cfg.VhostList.Where((x) => { return Regex.IsMatch(req.Header["Host"].Value, x.host) && x.port == Port; }).First();
                        var res = resmeth.HandleRequest(req, resmeth.www, resmeth.host, resmeth);
                        if (res.FlushOnClose)
                        {
                            if (!res.ConnectionOpen)
                            {
                                var sendbuffer = res.Flush();
                                Write(ns, sendbuffer);
                            }
                        }
                        else
                        {
                            var sendbuffer = res.Flush();
                            Write(ns, sendbuffer);
                        }

                        if (!res.ConnectionOpen)
                        {
                            ns.Flush();
                            ns.Close();
                            ns.Dispose();
                           
                            c.Close();
                            break;
                            
                        }

                    }
                Thread.Sleep(1);
            }

                Thread.CurrentThread.Abort();
            
        }

        private void Write(NetworkStream s, byte[] b)
        {

            s.Write(b, 0, b.Length);

            /*var stream = new MemoryStream(b);
            byte[] buffer = new byte[4096];
            while (true)
            {
                int space = 4096, read, offset = 0;
                while (space > 0 && (read = stream.Read(buffer, offset, space)) > 0)
                {
                    space -= read;
                    offset += read;
                }
                // either a full buffer, or EOF
                if (space != 0)
                { // EOF - final
                    if (offset != 0)
                    { // something to send
                        Array.Resize(ref buffer, offset);
                        s.Write(buffer, 0, buffer.Length);
                    }
                    return;
                }
                else { // full buffer
                    s.Write(buffer, 0, buffer.Length);
                }
            }*/
        }
    }
}
