using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals
{
    public static class SessionManager
    {
        public static string Create()
        {
            var guid = Guid.NewGuid().ToString();
            return guid;
        }

        public static Dictionary<string, string> Get(string guid)
        {
            var path = Path.Combine(Config.SessionDirectory, guid + ".ses");
            if(File.Exists(path))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        public static void Set(string guid, Dictionary<string,string> ses)
        {
            var path = Path.Combine(Config.SessionDirectory, guid + ".ses");
            File.WriteAllText(path, JsonConvert.SerializeObject(ses));
        }

    }
}
