using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Internals
{
    public static class Log
    {
        public static void Info(string msg)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("info");
            Console.ResetColor();
            Console.Write("]");
            Console.WriteLine(msg);
        }

        public static void Error(string msg)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("error");
            Console.ResetColor();
            Console.Write("]");
            Console.WriteLine(msg);
        }

    }
}
