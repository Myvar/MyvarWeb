using MyvarWeb.Net;
using MyvarWeb.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb
{
    public static class VhostEngine
    {
        public static HttpResponce GenerateResponce(HttpRequest req)
        {
            return new StringResponce("lol");
        }
    }
}
