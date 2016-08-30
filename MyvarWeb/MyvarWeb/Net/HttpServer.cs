using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public class HttpServer
    {
        //vhost config
        public int Port { get; set; }

        //execution
        public int MaxThreads = 4;

        //internals
        private List<Thread> Threads { get; set; } = new List<Thread>();
        private Thread NetworkT { get; set; }
        private Queue<TcpClient> ClientQueue { get; set; } = new Queue<TcpClient>();

        private object _locker = new object();

        public HttpServer(int port)
        {
            
            Port = port;

            NetworkT = new Thread(new ThreadStart(NetworkThread));
        }

        public void Start()
        {
            NetworkT.Start();

            for (int i = 0; i < MaxThreads; i++)
            {
                Threads.Add(new Thread(new ThreadStart(StartHandlerThread)));
            }

            foreach(var i in Threads)
            {
                i.Start();
            }
        }

        private void NetworkThread()
        {
            TcpListener tcp = new TcpListener(IPAddress.Any, Port);
            tcp.Start();
            while (true)
            {
                var s = tcp.AcceptTcpClient();

                lock (_locker)
                {
                    ClientQueue.Enqueue(s);
                }
                
            }
        }

        private void StartHandlerThread()
        {
            while(true)
            {
                TcpClient s = null;
                lock(_locker)
                {
                    if(ClientQueue.Count != 0)
                    {
                        s = ClientQueue.Dequeue();
                    }
                }

                if(s != null)
                {
                    var req = new HttpRequest(s);
                    req.Parse();
                    try
                    {
                        var resp = VhostEngine.GenerateResponce(req);
                        resp.Write(s.GetStream());
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    s.Close();
                    
                }

                Thread.Sleep(25);
            }
        }
    }
}
