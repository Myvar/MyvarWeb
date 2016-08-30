using MyvarWeb.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public class HttpHeader
    {
        private string _raw { get; set; }

        public List<HttpField> Fields { get; set; } = new List<HttpField>();

        public HttpHeader(string raw)
        {
            _raw = raw;

            foreach(var i in raw.Replace("\r\n", "\n").Trim().Split('\n'))
            {
                Fields.Add(HttpField.ParseGlobal(i));
            }
        }

        public HttpHeader()
        {
            Fields.Add(new HttpFieldHeader());
            Fields.Add(new DateHeader());
            Fields.Add(new ServerHeader());
            Fields.Add(new ConnectionHeader() { Connection = "Closed" });
        }

        public override string ToString()
        {
            /*return  "HTTP/1.1 200 Life is good" + Environment.NewLine +
              "Date: " + DateTime.Now + Environment.NewLine +
              "Server: " + "MyvarWeb/0.1" + Environment.NewLine +
              "Connection: " + "Closed";*/
            var re = new StringBuilder();

            foreach(var i in Fields)
            {
                re.AppendLine(i.ToString());
            }

            return re.ToString().Trim();
        }
    }
}
