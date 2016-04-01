using MW.Core.Internals;
using MW.Core.Internals.cgi;
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

        };

        public List<Vhost> VhostList { get; set; } = new List<Vhost>()
        {
            new Vhost(".+:80")
            {
                 www = "www",
                Cgi = new List<string>()
                {
                    "php",
                    "csharp"
                }
            }
        };

        public List<Cgi> CgiList { get; set; } = new List<Cgi>()
        {
            new Cgi()
            {
                CommandLine = "-q \"{file}\"",
                Exe = ".\\bin\\php\\php-cgi.exe",
                Name = "php",
                FileExtensions = new List<string>() { ".php" }
            },
            new Cgi()
            {
                CommandLine = "\"{file}\" -d",
                Exe = ".\\bin\\cs\\CsharpCgi.exe",
                Name = "csharp",
                FileExtensions = new List<string>() { ".cs" }
            }
        };

        public List<string> IndexTypes { get; set; } = new List<string>()
        {
            "index.cs",
            "index.php",
            "index.htm",
            "index.html"
        };


        public Config()
        {
            foreach (var i in typeof(Config).GetFields())
            {
                i.SetValue(this, Activator.CreateInstance(i.FieldType));
            }

            foreach (var i in typeof(Config).GetProperties())
            {
                try
                {
                    i.SetValue(this, Activator.CreateInstance(i.PropertyType));
                }
                catch
                {
                    i.SetValue(this, Activator.CreateInstance(i.PropertyType, new object[] { new char[] { '\0' } }));
                }
            }
        }

        public Config(bool b)
        {

        }

        [JsonIgnore]
        public static string ServerVertion { get; set; } = "0.0.2";
    }
}
