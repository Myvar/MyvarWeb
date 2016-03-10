using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core.Internals.cgi
{
    public class Cgi
    {
        public string Name { get; set; }
        public string Exe { get; set; }
        public string CommandLine { get; set; }
        public List<string> EnvironmentVariables { get; set; } = new List<string>();

        public List<string> FileExtensions { get; set; } = new List<string>();

    }
}
