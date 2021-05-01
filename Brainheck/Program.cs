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
            l.Name = "Tutirial 1";
            l.Task = "Write a program that adds 6 to the selected cell";
            l.Code = "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.> +.++++++ +..++ +.> ++.<< +++++++++++++++.>.++ +.------.--------.> +.>.".Replace(" "," ");

            l.DrawLayout();
            l.Run();
            Console.ReadLine();
        }
    }
}
