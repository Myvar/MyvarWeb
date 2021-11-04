using System.IO;
using System.Net.Sockets;

namespace Prototype.Http
{
    /// <summary>
    /// Used for threading
    /// </summary>
    public static class MemoryStore
    {
        public static ConcurrentStore<Stream> StreamStore { get; set; } = new ConcurrentStore<Stream>();
        public static ConcurrentStore<TcpClient> TcpStore { get; set; } = new ConcurrentStore<TcpClient>();
    }
}