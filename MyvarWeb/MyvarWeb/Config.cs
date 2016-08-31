using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb
{
    public class Config
    {
        public Dictionary<string, string> VhostTable { get; set; } = new Dictionary<string, string>();
        /*     {
                 {"localhost:8080", "./www"}
             };*/

        public List<int> PortIndex { get; set; } = new List<int>();
      /*  {
            8080
        };*/

        public List<string> DisabledPaths { get; set; } = new List<string>();
       /* {
            ""
        };*/
    }
}
