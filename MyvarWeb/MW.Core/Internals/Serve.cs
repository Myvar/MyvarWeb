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
            string contenttype = ".html";
            string path = "./" + www + req.Header.Fields[0].ID.Split(' ')[1].Split('?')[0];
            string query = req.Header.Fields[0].ID.Split(' ')[1].Split('?').Last();
            try
            {


                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var i in Globals.cfg.IndexTypes)
                    {
                        if (File.Exists(Path.Combine(path, i)))
                        {
                            var fli = new FileInfo(Path.Combine(path, i));
                            contenttype = Utils.GetExsentionType(fli.Extension);
                            res.AddRange(GetContent(Path.Combine(path, i), v, query));
                            break;
                        }
                    }
                }
            }
            catch
            { }
            try
            {

                if (File.Exists(path))
                {
                    var fli = new FileInfo(path);
                    contenttype = Utils.GetExsentionType(fli.Extension);
                    res.AddRange(GetContent(path, v, query));
                }


            }
            catch
            { }


            if (res.Count == 0)
            {
                return new HttpResponce("404 Page not found");
            }
            var resp = new HttpResponce() { Body = res.ToArray(), ConnectionOpen = false };
            resp.Header.Fields.Add(new HttpField("Content-Type", contenttype));
            resp.Header.Fields.Add(new HttpField("Content-Length", res.Count.ToString()));
            resp.Header.Fields.Add(new HttpField("Content-Range: bytes"));
            return resp;
        }

        public static byte[] GetContent(string path, Vhost v, string query)
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
                    p.StartInfo.Arguments = i.CommandLine.Replace("{file}", Path.GetFullPath(path).TrimEnd('/')) + " " + query.Split('#')[0].Replace("&", " s");
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                   // p.StartInfo.EnvironmentVariables["QUERY_STRING"] =  ;
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
