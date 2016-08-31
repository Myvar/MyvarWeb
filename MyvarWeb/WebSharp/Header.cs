using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp
{
    public class Header
    {
        private string _Headers;
        private Action<string> _WriteHeader;

        public string Raw { get; set; }
        public List<HttpField> Headers { get; set; } = new List<HttpField>();

        public T GetHeader<T>() where T : HttpField
        {
            foreach(var i in Headers)
            {
                if(i is T)
                {
                    return (T)i;
                }
            }

            return default(T);
        }

        public void AddHeader(string s)
        {
            _WriteHeader(s);
        }

        public void AddHeader(HttpField s)
        {
            _WriteHeader(s.ToString());
        }

        public Header(string _Headers)
        {
            this._Headers = _Headers;
            Raw = _Headers;
            foreach(var i in _Headers.Replace("\r\n", "\n").Split('\n'))
            {
                Headers.Add(HttpField.ParseGlobal(i));
            }
        }

        public Header(string _Headers, Action<string> _WriteHeader) : this(_Headers)
        {
            this._WriteHeader = _WriteHeader;
        }
    }
}
