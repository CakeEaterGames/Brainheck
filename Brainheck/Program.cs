using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            LevelUtils.Init();
            SaveData.LoadFromFile();

            lib.LevelSelectScreen();

 

          //  Level l = new Level("tut1");
          
            //l.Loop();
           Console.ReadLine();
        }
    }
}
