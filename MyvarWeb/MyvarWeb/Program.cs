using MyvarWeb.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Info("Server starting ...");

            HttpServer.Start();

            Log.Info("Server booted");

            while (true)
            {
                //wright console interface here
            }

        }
    }
}
