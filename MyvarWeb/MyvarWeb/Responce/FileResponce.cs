using MyvarWeb.Headers;
using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Responce
{
    public class FileResponce : HttpResponce
    {
        public string Content { get; set; }

        public FileResponce(string content)
        {
            Content = content;
        }

        public override void Write(NetworkStream ns)
        {
            {
                Headers.Fields.Add(new GenericHeader() { Raw = "Content-Type: " + GetExsentionType(new FileInfo(Content).Extension) });
                //Headers.Fields.Add(new GenericHeader() { Raw = "Content-Length: " + new FileInfo(Content).Length });
               // Headers.Fields.Add(new GenericHeader() { Raw = "Content-Range: bytes" });
                var buf = Encoding.ASCII.GetBytes(Headers.ToString() + Environment.NewLine + Environment.NewLine);
                ns.Write(buf, 0, buf.Length);
            }
            {

                FileStream fs = new FileStream(Content, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.CopyTo(ns);
                fs.Close();
            }
        }

        public static string GetExsentionType(string ex)
        {
            if (_mimeTypeMappings.ContainsKey(ex))
            {
                return _mimeTypeMappings[ex];
            }

            return "application/octet-stream";
        }

        #region extension to MIME type list
        private static Dictionary<string, string> _mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        {".cs", "text/html"},
        {".php", "text/html"},
        {".asf", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".avi", "video/x-msvideo"},
        {".bin", "application/octet-stream"},
        {".cco", "application/x-cocoa"},
        {".crt", "application/x-x509-ca-cert"},
        {".css", "text/css"},
        {".deb", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dll", "application/octet-stream"},
        {".dmg", "application/octet-stream"},
        {".ear", "application/java-archive"},
        {".eot", "application/octet-stream"},
        {".exe", "application/octet-stream"},
        {".flv", "video/x-flv"},
        {".gif", "image/gif"},
        {".hqx", "application/mac-binhex40"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".ico", "image/x-icon"},
        {".img", "application/octet-stream"},
        {".iso", "application/octet-stream"},
        {".jar", "application/java-archive"},
        {".jardiff", "application/x-java-archive-diff"},
        {".jng", "image/x-jng"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".mml", "text/mathml"},
        {".mng", "video/x-mng"},
        {".mov", "video/quicktime"},
        {".mp3", "audio/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpg", "video/mpeg"},
        {".msi", "application/octet-stream"},
        {".msm", "application/octet-stream"},
        {".msp", "application/octet-stream"},
        {".pdb", "application/x-pilot"},
        {".pdf", "application/pdf"},
        {".pem", "application/x-x509-ca-cert"},
        {".pl", "application/x-perl"},
        {".pm", "application/x-perl"},
        {".png", "image/png"},
        {".prc", "application/x-pilot"},
        {".ra", "audio/x-realaudio"},
        {".rar", "application/x-rar-compressed"},
        {".rpm", "application/x-redhat-package-manager"},
        {".rss", "text/xml"},
        {".run", "application/x-makeself"},
        {".sea", "application/x-sea"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".swf", "application/x-shockwave-flash"},
        {".tcl", "application/x-tcl"},
        {".tk", "application/x-tcl"},
        {".txt", "text/plain"},
        {".war", "application/java-archive"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wmv", "video/x-ms-wmv"},
        {".xml", "text/xml"},
        {".xpi", "application/x-xpinstall"},
        {".zip", "application/zip"},
        {".woff", "application/font-woff"},
        {".woff2", "application/font-woff2"},
        {".ttf", "application/x-font-ttf"},

    };
        #endregion
    }
}
