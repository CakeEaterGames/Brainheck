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
            LevelSelect.Init();
            SaveData.LoadFromFile();

            LevelSelect.Draw();

            Console.ReadLine();
        }
    }
}
