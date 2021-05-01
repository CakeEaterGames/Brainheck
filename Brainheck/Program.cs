using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    class Program
    {
        static void Main(string[] args)
        {
            /*

            LevelUtils.Init();
            SaveData.LoadFromFile();

            lib.LevelSelectScreen();

    */

            Level l = new Level();
            l.LoadFromRes("tut1");
            l.CreateFoulders("tut1");
            l.SetTest(0);
            l.LoadSolutionFromFile("tut1");
            l.DrawLayout();
            l.Run();
            Console.ReadLine();
        }
    }
}
