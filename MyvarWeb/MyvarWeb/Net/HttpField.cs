using MyvarWeb.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Net
{
    public abstract class HttpField
    {
        public abstract HttpField Parse(string s);
        public abstract bool Id(string s);
        public static List<HttpField> index { get; set; } = new List<HttpField>()
        {
            new MethodHeader(),
            new AcceptHeader(),
            new AcceptEncodingHeader(),
            new AcceptLanguageHeader(),
            new CacheControlHeader(),
            new ConnectionHeader(),
            new HostHeader(),
            new UserAgentHeader(),
            new GenericHeader() // always last in list
        };
        public static HttpField ParseGlobal(string s)
        {
            foreach(var i in index)
            {
                if (i.Id(s))
                {
                    return i.Parse(s);
                }
            }
            return null;
        }
       
    }
}
