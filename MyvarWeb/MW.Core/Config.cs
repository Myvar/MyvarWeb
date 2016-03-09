using MW.Core.Internals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core
{
    public class Config
    {
        public List<Proxy> ProxyList { get; set; } = new List<Proxy>();

        public List<Vhost> VhostList { get; set; } = new List<Vhost>();

        [JsonIgnore]
        public static string ServerVertion { get; set; } = "0.0.1";
    }
}
