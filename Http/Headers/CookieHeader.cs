using System;
using System.Collections.Generic;
using System.Linq;
using Prototype.Utils;

namespace Prototype.Http.Headers
{
    public class CookieHeader : HttpHeader
    {
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

        public override bool TryParse(string line, out HttpHeader header)
        {
            line = line.Trim();


            if (!line.ToLower().StartsWith("cookie:"))
            {
                header = null;
                return false;
            }

            var re = new CookieHeader();


            foreach (var cookie in line.Split(':').Last().Trim().Split(';'))
            {
                try
                {
                    var segs = cookie.Split('=');
                    re.Cookies.Add(segs[0].Trim(), segs[1].Trim());
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString());
                }
            }

            header = re;
            return true;
        }
    }
}