using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Тест_OpenTK
{
    class Log
    {
        static public void LogConsole (string s, double x, double y)
        {
            Console.WriteLine($"Console {s} {Math.Round(x, 3)} {Math.Round(y, 3)}{Environment.NewLine}");
        }
        static public void LogFile(string s, double x, double y)
        {
            System.IO.File.AppendAllText("log.txt", $"{DateTime.Now} {s} {Math.Round(x, 3)} {Math.Round(y, 3)}{Environment.NewLine}");
        }
    }
}
