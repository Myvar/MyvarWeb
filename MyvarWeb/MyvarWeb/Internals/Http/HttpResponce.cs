using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals.Http
{
    public class HttpResponce
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>()
        {
            {"Date", DateTime.Now.ToString()},
            {"Server", "MyvarWeb/" + Config.ServerVertion},
            {"Connection", "Closed"}
        };

        public string Body { get; set; } = "";


        public string HeadersToString()
        {
            StringBuilder re = new StringBuilder();

            re.AppendLine("HTTP/1.1 200 OK");

            foreach(var i in Headers)
            {
                re.Append(i.Key);
                re.Append(": ");
                re.AppendLine(i.Value);
            }
            re.AppendLine(); 
            return re.ToString();
        }

        public override string ToString()
        {
            return Body;
        }
    }
}
