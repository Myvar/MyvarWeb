using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class HttpHeader
    {
        public List<HttpField> Fields = new List<HttpField>() {
            new HttpField("HTTP/1.1 200 OK"),
            new HttpField("Date", DateTime.Now.ToString()),
            new HttpField("Server", "MyvarWeb/" + Config.ServerVertion),
            new HttpField("Connection", "Closed")
        };

        public HttpField this [string key]
        {
            get {
                return Fields.Where((x) => { return x.ID == key; }).First();
            }
            set
            {
                Fields[Fields.IndexOf(Fields.Where((x) => { return x.ID == key; }).First())] = value;
            }
        }

        public HttpHeader()
        {

        }

        public HttpHeader(byte[] data)
        {
            Fields = new List<HttpField>();

            var x = Regex.Split(Utils.GetString(data), "\r\n\r\n")[0];
            foreach (var i in x.Replace("\r\n", "\n").Split('\n'))
            {
                var s1 = Regex.Split(i , ":\\s");
                if (s1.Length == 2)
                {
                    Fields.Add(new HttpField(s1[0].Trim(), s1[1].Trim()));
                }
                else
                {
                    Fields.Add(new HttpField(i));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var i in Fields)
            {
                sb.AppendLine(i.ToString());
            }

            sb.AppendLine();


            return sb.ToString();
        }
    }

    public class HttpField
    {
        public string ID { get; set; }
        public string Value { get; set; }
        public string Joiner { get; set; } = ":";

        public HttpField(string id, string value)
        {
            ID = id;
            Value = value;
        }

        public HttpField(string nobinder)
        {
            ID = nobinder;
            Joiner = "";
        }

        public override string ToString()
        {
            return ID + Joiner + Value;
        }
    }
}
