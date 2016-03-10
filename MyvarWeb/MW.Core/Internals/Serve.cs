using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MW.Core.Internals
{
    public class Serve
    {
        public static HttpResponce Serveuri(HttpRequest req, string www, string host, Vhost v)
        {
            var res = new List<byte>();
            string path = "./" + www + req.Header.Fields[0].ID.Split(' ')[1];
            try
            {


                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var i in Globals.cfg.IndexTypes)
                    {
                        if (File.Exists(Path.Combine(path, i)))
                        {
                            res.AddRange(GetContent(Path.Combine(path, i), v));
                            break;
                        }
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        res.AddRange(GetContent(path, v));
                    }
                }
            }
            catch
            { }

            
            if (res.Count == 0)
            {
                return new HttpResponce("404 Page not found");
            }
            return new HttpResponce() { Body = res.ToArray(), ConnectionOpen = false };
        }

        public static byte[] GetContent(string path, Vhost v)
        {
            byte[] re = File.ReadAllBytes(path);

            var fl = new FileInfo(path);

            foreach (var i in Globals.cfg.CgiList)
            {
                if (i.FileExtensions.Contains(fl.Extension) && v.Cgi.Contains(i.Name))
                {
                    var p = new Process();
                    p.StartInfo.FileName = Path.GetFullPath(i.Exe);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.Arguments = i.CommandLine.Replace("{file}", Path.GetFullPath(path).TrimEnd('/')) ;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;

                    p.Start();
                    
                    string sOutput = p.StandardOutput.ReadToEnd();
                    re = Utils.GetBytes(sOutput);

                    p.WaitForExit();
                    p.Close();

                    break;
                }
            }

            return re;
        }
    }

}
