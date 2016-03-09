using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.Core.Util
{
    public static class Logger
    {
        public static bool ConsoleEnabled { get; set; } = true;

        private static readonly object _LockObject = new object();

        public static void Raw(string msg)
        {
            lock (_LockObject)
            {               
                Console.WriteLine(msg);
            }
        }

        public static void Log(string msg)
        {
            if (ConsoleEnabled)
            {
                lock(_LockObject)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("LOG");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("]");
                    Console.ResetColor();
                    Console.WriteLine(msg);
                }
            }
        }

        public static void Warning(string msg)
        {
            if (ConsoleEnabled)
            {
                lock (_LockObject)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Warning");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("]");
                    Console.ResetColor();
                    Console.WriteLine(msg);
                }
            }
        }

        public static void Error(string msg)
        {
            if (ConsoleEnabled)
            {
                lock (_LockObject)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("]");
                    Console.ResetColor();
                    Console.WriteLine(msg);
                }
            }
        }
    }
}
