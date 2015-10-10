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

            if (!Directory.Exists(Config.SessionDirectory))
            {
                Directory.CreateDirectory(Config.SessionDirectory);
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
            var guid = "";
            if (!req.Headers.ContainsKey("Cookie"))
            {
                var x = SessionManager.Create();
                re.Headers.Add("Set-Cookie", "sid=" + x + "; expires=" + DateTime.Now.AddYears(10).ToUniversalTime().ToString("r"));
                guid = x;
            }
            else
            {
                guid = req.Headers["Cookie"].Split(';')[0].Split('=')[1].Trim();
            }

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
                    var x = Compiler.Execute(filepath, Utils.BuildGetOrPost(req.Uri.Query), Utils.BuildGetOrPost(req.Body), GetBank(Path.Combine(Config.Databank, req.Uri.Host + ".bank")), filepath, SessionManager.Get(guid));
                    re.Body = Utils.GetBytes(x[0] as string);
                    re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(filepath).Extension));
                    dynamic z = x[1];
                    SessionManager.Set(guid, z);
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
                        var x = Compiler.Execute(newf, Utils.BuildGetOrPost(req.Uri.Query), Utils.BuildGetOrPost(req.Body), GetBank(Path.Combine(Config.Databank, req.Uri.Host + ".bank")), filepath, SessionManager.Get(guid));
                        re.Body = Utils.GetBytes(x[0] as string);
                        re.Headers.Add("Content-Type", Utils.GetExsentionType(new FileInfo(newf).Extension));
                        dynamic z = x[1];
                        SessionManager.Set(guid, z);
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
