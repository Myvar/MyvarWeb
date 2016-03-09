using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class Proxy
    {
      

        public int Aport { get; set; }
        public int Bport { get; set; }
        public string Bhost { get; set; }

        public Proxy(int aport, string bhost, int bport)
        {
            Aport = aport;
            Bport = bport;
            Bhost = bhost;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem((xyz) =>
            {
                var x = new TcpForwarderSlim();
                var ip = Dns.GetHostEntry(Bhost);
                x.Start(new IPEndPoint(IPAddress.Any, Aport), new IPEndPoint(ip.AddressList.First(), Bport));
            });
            
        }
    }

    public class TcpForwarderSlim
    {
        private readonly Socket MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public void Start(IPEndPoint local, IPEndPoint remote)
        {
            
            MainSocket.Bind(local);
            MainSocket.Listen(10);

            while (true)
            {
                var source = MainSocket.Accept();
                var destination = new TcpForwarderSlim();
                var state = new State(source, destination.MainSocket);
                destination.Connect(remote, source);
                source.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
            }
        }

        private void Connect(EndPoint remoteEndpoint, Socket destination)
        {
            var state = new State(MainSocket, destination);
            MainSocket.Connect(remoteEndpoint);
            MainSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, OnDataReceive, state);
        }

        private static void OnDataReceive(IAsyncResult result)
        {
            var state = (State)result.AsyncState;
            try
            {
                var bytesRead = state.SourceSocket.EndReceive(result);
                if (bytesRead > 0)
                {
                    state.DestinationSocket.Send(state.Buffer, bytesRead, SocketFlags.None);
                    state.SourceSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
                }
            }
            catch
            {
                state.DestinationSocket.Close();
                state.SourceSocket.Close();
            }
        }

        private class State
        {
            public Socket SourceSocket { get; private set; }
            public Socket DestinationSocket { get; private set; }
            public byte[] Buffer { get; private set; }

            public State(Socket source, Socket destination)
            {
                SourceSocket = source;
                DestinationSocket = destination;
                Buffer = new byte[8192];
            }
        }
    }
}
