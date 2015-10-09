using MyvarWeb.Internals.Http;
using MyvarWeb.Scripted;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals
{
    public static class HttpServer
    {
        private static NetworkManager _nm { get; set; } = new NetworkManager();

        public static void Start()
        {
            StartUp();
            _nm.Handler = HandleReq;
            _nm.Start();
        }

        private static void StartUp()
        {
            if(!Directory.Exists(Config.RootDirectory))
            {
                Directory.CreateDirectory(Config.RootDirectory);
            }

            if (!Directory.Exists(Config.Databank))
            {
                Directory.CreateDirectory(Config.Databank);
            }

            Compiler.DebugMode = true;
            Compiler.ReCompile = true;
        }

        public static void Stop()
        {
            _nm.Stop();
        }

        public static HttpResponce HandleReq(HttpRequest req)
        {
            var re = new HttpResponce();
            re.Body = Utils.GetBytes("404 Page not found");

            var path = req.Uri.Host + req.Uri.LocalPath;
            var filepath = Path.Combine(Config.RootDirectory, path);
            if (File.Exists(filepath))
            {
                if (!filepath.EndsWith(".cs"))
                {
                    re.Body = File.ReadAllBytes(filepath);
                    re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(filepath).Extension));
                }
                else
                {
                    re.Body = Utils.GetBytes(Compiler.Execute(filepath, Utils.BuildGetOrPost(req.Uri.Query), Utils.BuildGetOrPost(req.Body), GetBank(Path.Combine(Config.Databank, req.Uri.Host + ".bank")) ) as string);
                    re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(filepath).Extension));
                }
            }
            else
            {
                string newf = "";
                if(File.Exists(Path.Combine(filepath,"index.cs")))
                {
                    newf = Path.Combine(filepath, "index.cs");
                }
                if (File.Exists(Path.Combine(filepath, "index.html")))
                {
                    newf = Path.Combine(filepath, "index.html");
                }
                if (File.Exists(newf))
                {
                    if (!newf.EndsWith(".cs"))
                    {
                        re.Body = File.ReadAllBytes(newf);
                        re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(newf).Extension));
                    }
                    else
                    {
                        re.Body = Utils.GetBytes(Compiler.Execute(newf, Utils.BuildGetOrPost(req.Uri.Query), Utils.BuildGetOrPost(req.Body), GetBank(Path.Combine(Config.Databank, req.Uri.Host + ".bank"))) as string);
                        re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(newf).Extension));
                    }
                }
            }

            return re;
        }

        private static  Databank.Databank GetBank(string p)
        {
            if(File.Exists(p))
            {
                return Databank.Databank.Load(p);
            }
            else
            {
                return new Databank.Databank(p);
            }
        }

    }
}
