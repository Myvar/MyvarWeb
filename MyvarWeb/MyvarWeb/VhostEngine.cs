using MyvarWeb.Net;
using MyvarWeb.Responce;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb
{
    public static class VhostEngine
    {
        public static HttpResponce GenerateResponce(HttpRequest req)
        {
            var path = req.GetPath().Split('?')[0];
            var root = "";

            var hst = req.GetHost();
            if (Program.Cfg.VhostTable.ContainsKey(hst))
            {
                root = Program.Cfg.VhostTable[hst];
            }
            else
            {
                root = Program.Cfg.VhostTable.Values.First();
            }

            string file = "";

            if (path == "/")
            {
                //hard coded for now
                if (File.Exists(root.TrimEnd('\\').TrimEnd('/') + path + "index.html"))
                {
                    file = root.TrimEnd('\\').TrimEnd('/') + path + "index.html";
                }
                if (File.Exists(root.TrimEnd('\\').TrimEnd('/') + path + "index.cs"))
                {
                    file = root.TrimEnd('\\').TrimEnd('/') + path + "index.cs";
                }
            }

            if (File.Exists(root.TrimEnd('\\').TrimEnd('/') + path))
            {
                file = root.TrimEnd('\\').TrimEnd('/') + path;
            }

            if(!string.IsNullOrEmpty(file))
            {
                file = Path.GetFullPath(file);
                if(file.StartsWith(Path.GetFullPath(root)))// makes sure user can acces file system with ../
                {
                    if(file.EndsWith(".cs"))
                    {
                        return new WebSharpResponce(file);
                    }
                    else
                    {
                        return new FileResponce(file);
                    }
                }
            }

            return new StringResponce("404");
        }
    }
}
