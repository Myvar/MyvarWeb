using MW.Core.Internals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core
{
    public class Engine
    {
        public void Start()
        {

            if(File.Exists("config.json"))
            {
                Globals.cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            }
            else
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(Globals.cfg, Formatting.Indented));
            }

            var webserver = new Http();
            webserver.Start();
            while (true) ; // dont kill main thread
        }
    }
}
