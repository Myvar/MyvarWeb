using Microsoft.CodeAnalysis.CSharp.Scripting;
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

        public WebSharpInterpiter(string src)
        {
            Raw = src;
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
                    script.Options.AddImports("WebSharp");
                    script.Options.AddImports("System");
                    //script.Options.AddReferences(typeof(System.DateTime).Assembly);
                    script.Compile();
                    var x = script.RunAsync(globals: new Core(_ret)).Result;
                    
                }
            }
        }

        public string Gencontent()
        {
            var ts = Parse(Raw);
            Interpite(ts);

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
