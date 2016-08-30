using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp
{
    public class Core
    {
        private Action<string> Write { get; set; }

        public Core(StringBuilder sb)
        {
            Write = (x) => { sb.Append(x); };
        }

        public void echo(string s)
        {
            Write(s);
        }
    }
}
