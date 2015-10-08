using MyvarWeb.Internals.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace MyvarWeb.Internals
{
    public class NetworkManager
    {
        public Func<HttpRequest, HttpResponce> Handler { get; set; }

        private TcpListener _listener { get; set; } 

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any ,Config.Port);
            _listener.Start();

            ThreadPool.QueueUserWorkItem((x) => {
                var list = x as TcpListener;

                while(true)
                {
                    var g = list.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem((xy) => {
                        var tcp = xy as TcpClient;
                        var ns = tcp.GetStream();

                        while (tcp.Connected)
                        {
                            if (ns.DataAvailable)
                            {
                                byte[] buffer = new byte[4096];
                                int bytesread = ns.Read(buffer, 0, buffer.Length);
                                Array.Resize(ref buffer, bytesread); //Cut it to be the actual message.

                                var res = Handler(HttpRequest.Parse(buffer));

                                //wright respond
                                //headers
                                var headers = Encoding.UTF8.GetBytes(res.HeadersToString());
                                ns.Write(headers, 0, headers.Length);
                                //body
                                var body = Encoding.UTF8.GetBytes(res.ToString());
                                ns.Write(body, 0, body.Length);

                                //close the connection
                                ns.Flush();
                                ns.Close();
                                tcp.Close();
                            }
                        }

                    }, g);
                }

            }, _listener);
        }
    }
}
