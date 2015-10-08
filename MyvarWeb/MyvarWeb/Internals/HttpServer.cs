using MyvarWeb.Internals.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals
{
    public static class HttpServer
    {
        private static NetworkManager _nm { get; set; } = new NetworkManager();

        public static void Start()
        {
            _nm.Handler = HandleReq;
            _nm.Start();
        }

        public static void Stop()
        {

        }

        public static HttpResponce HandleReq(HttpRequest req)
        {
            return new HttpResponce() { Body = "test" };
        }

    }
}
