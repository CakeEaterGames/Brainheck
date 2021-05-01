using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    public static class lib
    {
        static string PressEnter = "Press Enter to continue";
        public static bool ToClear = true;
        public static void Dialog(string[] lines)
        {
            foreach (var l in lines)
            {
                if (ToClear) Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var c in l)
                {
                    if(c!=' ')
                    Console.Beep(1000, 1);
                    Thread.Sleep(5);
                    Console.Write(c);
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(PressEnter);
                Console.ReadLine();
            }
        }

        public static void DialogRes(string fileName)
        {
            Dialog(lib.ResText(fileName).Replace("\r","").Split('\n'));
        }

        public static void LevelSelectScreen()
        {
            LevelGroup("Getting started");
            LevelTitle(LevelName.Intro);
            LevelTitle(LevelName.tut1);
            LevelTitle(LevelName.tut2);
            LevelTitle(LevelName.tut3);
            LevelTitle(LevelName.tut4);
            LevelTitle(LevelName.tut5);

            Console.WriteLine();
            LevelSelectPrompt();

        }
        public static void LevelTitle(LevelName name)
        {
            if (LevelUtils.IsUnlocked(name))
            {
                var s = LevelUtils.LevelData[name];
                LevelTitle(
                    s.Title,
                    SaveData.GetBool(name.ToString() + LevelStat.IsSolved.ToString()),
                    SaveData.GetBool(name.ToString() + LevelStat.IsFast.ToString()),
                    SaveData.GetBool(name.ToString() + LevelStat.IsSmall.ToString()),
                    s.Id,
                    s.Color
                );
            }
        }


        public static void LevelTitle(string name,bool isDone, bool isFast, bool isShort, string code ,ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(code);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

            Console.Write("[");

            if (isDone)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("+");
        
            }
            else
            {
                Console.Write(" ");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (isFast) Console.Write(">");
            if (isShort) Console.Write(">");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");

            Console.ForegroundColor = color;
            Console.Write(name);
            Console.WriteLine();
        }

        public static void LevelSelectPrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("To select a level type it's ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("id ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("and press Enter");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LevelGroup(string name)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine(" "+name);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string ResText(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Brainheck."+path;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
            throw new Exception("No such text file in res");
        }
    }
}
