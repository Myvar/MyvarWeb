using MyvarWeb.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyvarWeb
{
    class Program
    {
        public static Config Cfg { get; set; } = new Config();

        static void Main(string[] args)
        {
            if(File.Exists("config.json"))
            {
                Cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            }
            else
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(Cfg, Formatting.Indented));
            }

            foreach (var i in Cfg.PortIndex)
            {
                HttpServer s = new HttpServer(i);
                s.Start();
            }
            while(true)
            {
                Thread.Sleep(25);
            }
        }
    }
}
