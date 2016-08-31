using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp.Headers;

namespace WebSharp
{
    public class Session
    {
        private Header header;
        private Action<string> _WriteHeader;
        private string file;
        private bool FoundCookie = false;
        private Dictionary<string, object> Objects { get; set; } = new Dictionary<string, object>();


        public Session(Header header, Action<string> _WriteHeader)
        {
            this.header = header;
            this._WriteHeader = _WriteHeader;

            if (!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }

            file = ".\\tmp\\";

            foreach (var i in header.Headers)
            {
                if (i is CookieHeader)
                {
                    var x = i as CookieHeader;
                    foreach (var cookie in x.Cookies.Split(';'))
                    {
                        var co = cookie.Trim().Split('=');
                        if (co[0] == "sid")
                        {
                            FoundCookie = true;
                            file += co[1] + ".json";
                        }
                    }

                    break;
                }
            }
        }

        public dynamic this[string s]
        {
            get
            {
                if (!FoundCookie)
                {
                    Start();
                }
                LoadSesstion();
                if (Objects.ContainsKey(s))
                {
                    return Objects[s];
                }

                return false;
            }
            set
            {
                if (!FoundCookie)
                {
                    Start();
                }
                
                if (Objects.ContainsKey(s))
                {
                    Objects[s] = value;
                }
                else
                {
                    Objects.Add(s, value);
                }
                SaveSesstion();
            }
        }

        public void Start()
        {
            if (!FoundCookie)
            {
                var s = Guid.NewGuid();
                _WriteHeader("Set-Cookie: sid=" + s);
                FoundCookie = true;
                file += s + ".json";
                SaveSesstion();
            }
        }

        private void SaveSesstion()
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(Objects));
        }

        private void LoadSesstion()
        {
            Objects = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(file));
        }
    }
}
