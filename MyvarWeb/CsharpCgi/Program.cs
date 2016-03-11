using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpCgi
{
    public class Program
    {
        static void Main(string[] args)
        {
            Compiler.ReCompile = true;
            Compiler.DebugMode = args[1] == "-d" ? true : false;
            Console.WriteLine(Compiler.Execute(args[0], new Dictionary<string, string>(), new Dictionary<string, string>(), new FileInfo(args[0]).DirectoryName)[0].ToString());
        }
    }
}
