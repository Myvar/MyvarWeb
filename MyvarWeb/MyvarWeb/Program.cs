using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer s = new HttpServer(8080);
            s.Start();

            while(true)
            {
                Thread.Sleep(25);
            }
        }
    }
}
