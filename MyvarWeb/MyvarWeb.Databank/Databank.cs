using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Databank
{
    public class Databank
    {
        public Dictionary<string, object> _data { get; set; } = new Dictionary<string, object>();
        public string _path { get; set; } = "";

        public Databank(string p)
        {
            _path = p;
        }


        public void Set(string nm, object value)
        {
            _data.Add(nm, value);
            Save(_path, this);
        }

        public T Get<T>(string name)
        {
            return (T)Convert.ChangeType(_data[name], typeof(T));
        }

        public static Databank Load(string fl)
        {
            return JsonConvert.DeserializeObject<Databank>(File.ReadAllText(fl));
        }

        public static void Save(string fl, Databank db)
        {
            var x = JsonConvert.SerializeObject(db );
            File.WriteAllText(fl, x);
        }

    }
}
