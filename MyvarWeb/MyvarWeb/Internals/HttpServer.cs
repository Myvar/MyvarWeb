using MyvarWeb.Internals.Http;
using System;
using System.Collections.Generic;
using System.IO;
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
            StartUp();
            _nm.Handler = HandleReq;
            _nm.Start();
        }

        private static void StartUp()
        {
            if(!Directory.Exists(Config.RootDirectory))
            {
                Directory.CreateDirectory(Config.RootDirectory);
            }
        }

        public static void Stop()
        {
            _nm.Stop();
        }

        public static HttpResponce HandleReq(HttpRequest req)
        {
            var re = new HttpResponce();
            re.Body = Utils.GetBytes("404 Page not found");

            if(File.Exists(req.Uri.LocalPath))
            {
                re.Body = File.ReadAllBytes(req.Uri.LocalPath);
            }

            return re;
        }

    }
}
