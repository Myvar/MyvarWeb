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

        public static HttpRequest Parse(byte[] buffer)
        {
            var raw = Encoding.UTF8.GetString(buffer);
            var re = new HttpRequest();

            foreach (var i in raw.Replace("\r\n", "\n").Split('\n'))
            {
                if (!string.IsNullOrEmpty(i))
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
            }
            re.Uri = new Uri("http://" + re.Headers["Host"] + re.Headers["GET"]);
            return re;
        }
    }
}
