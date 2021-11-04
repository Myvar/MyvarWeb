using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Prototype.Utils;

namespace Prototype.Http
{
    /// <summary>
    /// Http Server base impl
    /// </summary>
    public static class HttpServer
    {
        private static bool _running;
        private static List<TcpListener> _listeners = new List<TcpListener>();
        private static List<Thread> _workerThreads = new List<Thread>();
        private static List<Thread> _networkThreads = new List<Thread>();

        private static ConcurrentQueue<TcpClient> _clientQueue = new ConcurrentQueue<TcpClient>();

        /// <summary>
        /// Global Handler for incoming requests
        /// </summary>
        public static Func<HttpRequest, HttpResponse> Handler;

        /// <summary>
        /// is the server running
        /// </summary>
        public static bool Running
        {
            get => _running;
        }

        /// <summary>
        /// start the http server, make sure you add ports first
        /// </summary>
        public static void Start()
        {
            if (_running) return;
            _running = true;
            StartThreads();
        }

        /// <summary>
        /// stop the http server
        /// </summary>
        public static void Stop()
        {
            if (!_running) return;
            _running = false;

            foreach (var thread in _workerThreads)
            {
                thread.Join();
            }

            foreach (var thread in _networkThreads)
            {
                thread.Join();
            }

            _workerThreads.Clear();
            _networkThreads.Clear();
        }


        private static void StartThreads()
        {
            //@HACK @BUG this needs to be fixed and reworked based on amount of cores not hard coded

            var cores = Environment.ProcessorCount;

            for (int i = 0; i < cores; i++)
            {
                _workerThreads.Add(new Thread(WorkerThread));
            }

            for (int i = 0; i < cores; i++)
            {
                _networkThreads.Add(new Thread(NetworkThread));
            }

            foreach (var thread in _workerThreads)
            {
                thread.Start();
            }

            foreach (var thread in _networkThreads)
            {
                thread.Start();
            }
        }


        /// <summary>
        /// Add a port to listen on, Note: this does not start the server you still have to do that your self
        /// </summary>
        /// <param name="port">tcp port</param>
        /// <exception cref="Exception">Server must be stopped to add a port</exception>
        public static void AddPort(int port)
        {
            if (_running) throw new Exception("Add ports before you start the server ...");

            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            _listeners.Add(listener);
        }

        public static void RemovePort(int port)
        {
            if (_running) throw new Exception("Remove ports after you stop the server ...");

            for (var i = 0; i < _listeners.Count; i++)
            {
                var tcpListener = _listeners[i];
                if (((IPEndPoint) tcpListener.LocalEndpoint).Port == port)
                {
                    tcpListener.Stop();
                    _listeners.Remove(tcpListener);
                    break;
                }
            }
        }

        private static void WorkerThread()
        {
            while (_running)
            {
                foreach (var tcpListener in _listeners)
                {
                    if (tcpListener.Pending())
                    {
                        _clientQueue.Enqueue(tcpListener.AcceptTcpClient());
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
            }
        }

        private static void NetworkThread()
        {
            while (_running)
            {
                if (_clientQueue.IsEmpty)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    TcpClient client;
                    if (_clientQueue.TryDequeue(out client))
                    {
                        using (client)
                        using (var ns = client.GetStream())
                        using (var sr = new StreamReader(ns))
                        using (var sw = new StreamWriter(ns))
                        {
                            try
                            {
                                while (!ns.DataAvailable)
                                {
                                    Thread.Sleep(1);
                                }


                                var httpRequest = new HttpRequest();
                                httpRequest.ReadFromStream(ns);

                                var httResponse = Handler(httpRequest);

                                if (httResponse == null)
                                {
                                    new HttpResponse("404").Flush(sw);
                                }
                                else
                                {
                                    httResponse.Flush(sw);
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Error(e.ToString());
                            }


                            ns?.Close();
                            client?.Close();
                        }
                    }
                }
            }
        }
    }
}