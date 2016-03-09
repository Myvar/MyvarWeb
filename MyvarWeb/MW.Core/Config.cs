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
        public List<Proxy> ProxyList { get; set; } = new List<Proxy>()
        {
            new Proxy(8080, "git.myvar.online",  80)
        };

        public List<Vhost> VhostList { get; set; } = new List<Vhost>()
        {
            new Vhost("localhost:80")
            {
                 HandleRequest = (x) => { return  new HttpResponce("localhost " + DateTime.Now.ToString()); }
            },
            new Vhost("127.0.0.1:80")
            {
                 HandleRequest = (x) => { return  new HttpResponce("127.0.0.1 " + DateTime.Now.ToString()); }
            },
            new Vhost("127.0.0.1:8081")
            {
                 HandleRequest = (x) => { return  new HttpResponce("127.0.0.1:8081 " + DateTime.Now.ToString()); }
            }
        };

        [JsonIgnore]
        public static string ServerVertion { get; set; } = "0.0.1";
    }
}
