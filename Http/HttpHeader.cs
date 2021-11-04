using System;
using System.Collections.Generic;

namespace Prototype.Http
{
    public class HttpHeader
    {
        private static List<HttpHeader> Headers { get; set; } = new List<HttpHeader>();

        static HttpHeader()
        {
            foreach (var type in typeof(HttpHeader).Assembly.GetTypes())
            {
                if (type.BaseType == typeof(HttpHeader))
                {
                    Headers.Add((HttpHeader) Activator.CreateInstance(type));
                }
            }
        }

        public static HttpHeader Parse(string raw)
        {
            HttpHeader re = null;
            foreach (var header in Headers)
            {
                if (header.TryParse(raw, out re))
                {
                    break;
                }
            }

            return re;
        }

        public virtual bool TryParse(string line, out HttpHeader header)
        {
            header = null;
            return false;
        }
    }
}