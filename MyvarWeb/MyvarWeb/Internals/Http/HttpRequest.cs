using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals.Http
{
    public class HttpRequest
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Uri Uri { get; set; }
        public string Body { get; set; }

        public static HttpRequest Parse(byte[] buffer)
        {
            var raw = Encoding.UTF8.GetString(buffer);
            var re = new HttpRequest();

            bool Inbody = false;
            foreach (var i in raw.Replace("\r\n", "\n").Split('\n'))
            {
                if (!string.IsNullOrEmpty(i))
                {
                    if (!Inbody)
                    {
                        
                        var x = i.Split(':');
                        if (x.Length == 2)
                        {
                            re.Headers.Add(x[0].Trim(), x[1].Trim());
                        }
                        else
                        {
                            x = i.Split(' ');
                            if (x.Length == 3)
                            {
                                re.Headers.Add(x[0].Trim(), x[1].Trim());
                            }
                            else
                            {
                                re.Headers.Add(i, "");
                            }
                        }
                    }
                    else
                    {
                        re.Body += i + "\n";
                    }
                }
                else
                {
                    Inbody = true;
                }
            }
            try
            {

                re.Uri = new Uri("http://" + re.Headers["Host"] + re.Headers["GET"]);
            }
            catch {
                re.Uri = new Uri("http://" + re.Headers["Host"] + re.Headers["POST"]);
            }
            return re;
        }
    }
}
