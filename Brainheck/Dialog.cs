using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    static class Dialog
    {
        static string PressEnter = "Press Any Key to continue or press Escape to skip";
        public static bool ToClear = true;
        public static void FromArray(string[] lines)
        {
            foreach (var line in lines)
            {
                var l = line.Replace("\\n", "\n");
                if (ToClear) Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var c in l)
                {
                    if (c != ' ')
                        Console.Beep(1000, 1);
                    Thread.Sleep(5);
                    Console.Write(c);
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(PressEnter);
                var k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }


        }

        public static void FromRes(string fileName)
        {
            FromArray(lib.ResText(fileName).Replace("\r", "").Split('\n'));
        }
    }
}
