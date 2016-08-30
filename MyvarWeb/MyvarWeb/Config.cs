using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb
{
    public class Config
    {
        public Dictionary<string, string> VhostTable { get; set; } = new Dictionary<string, string>()
        {
            {"localhost:8080", "./www"}
        };
    }
}
