using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp
{
    public class Core
    {
        private Action<string> _Write { get; set; }
        private Action<string> _WriteHeader { get; set; }
        private string _Headers { get; set; }
        private string _Body { get; set; }

        public Header Header { get; set; }
        public Session Session { get; set; }

        public Core(StringBuilder sb, Action<string> writeHeader, string headers, string body)
        {
            _WriteHeader = writeHeader;
            _Write = (x) => { sb.Append(x); };
            _Headers = headers;
            _Body = body;

            Header = new Header(_Headers, _WriteHeader);
            Session = new Session(Header, _WriteHeader);
        }

        public void echo(string s)
        {
            _Write(s);
        }
    }
}
