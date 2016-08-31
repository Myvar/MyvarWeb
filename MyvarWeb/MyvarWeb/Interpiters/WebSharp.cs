using Microsoft.CodeAnalysis.CSharp.Scripting;
using MyvarWeb.Headers;
using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharp;

namespace MyvarWeb.Interpiters
{
    public class WebSharpInterpiter
    {
        public string Raw { get; set; }

        private StringBuilder _ret { get; set; } = new StringBuilder();
        public HttpHeader Headers { get; set; } = new HttpHeader();
        public HttpRequest ReqHeaders { get; set; } 

        private Action<string> WriteHeader { get; set; }

        public WebSharpInterpiter(string src, HttpHeader h, HttpRequest rh)
        {
            Headers.Fields.Clear();
            Raw = src;
            ReqHeaders = rh;
            WriteHeader = (s) => {
                Headers.Fields.Add(new GenericHeader() { Raw = s });
            };
        }

        public List<Token> Parse(string raw)
        {
            var re = new List<Token>();

            var t = new Token();
            var tmp = new StringBuilder();
            for (int c = 0; c < raw.Length; c++)
            {                
                if(raw[c] == '<' && raw[c + 1] == '?' && raw[c + 2] == 'c' && raw[c + 3] == 's')
                {
                    t.Raw = tmp.ToString();
                    tmp.Clear();
                    re.Add(t);
                    t = new Token();
                    t.Type = Type.Interpit;
                    c += 4;
                }
                else if (raw[c] == '?' && raw[c + 1] == '>')
                {
                    c += 2;
                    t.Raw = tmp.ToString();
                    tmp.Clear();
                    re.Add(t);
                    t = new Token();
                }
                else
                {
                    tmp.Append(raw[c]);
                }
            }
            t.Raw = tmp.ToString();
            re.Add(t);

            return re;              
        }

        public void Interpite(List<Token> ts)
        {
            foreach (var i in ts)
            {
                if (i.Type == Type.Append)
                {
                    _ret.Append(i.Raw);
                }
                else
                {

                    var script = CSharpScript.Create(i.Raw, globalsType: typeof(Core));
                    script.Options.AddReferences(typeof(WebSharp.Headers.CookieHeader).Assembly);
                    script.Options.AddImports("WebSharp");

                    script.Options.AddImports("System");
                    
                    script.Compile();
                    var x = script.RunAsync(globals: new Core(_ret, WriteHeader, ReqHeaders.Header.ToString(), ReqHeaders.Body)).Result;
                    
                }
            }
        }

        public string Gencontent()
        {
            var ts = Parse(Raw);
            try
            {
                Interpite(ts);
            }
            catch(Exception e)
            {
                return e.ToString();
            }

            return _ret.ToString();
        }
    }

    public class Token
    {
        public string Raw { get; set; }
        public Type Type { get; set; }
    }

    public enum Type
    {
        Append,
        Interpit
    }

}
