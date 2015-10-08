using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyvarWeb
{
    public static class Config
    {
        public static int Port { get; set; } = 80;
        public static string RootDirectory { get; set; } = "www";

        [JsonIgnore]
        public static string ServerVertion { get; set; } = "0.0.1";
    }
}
