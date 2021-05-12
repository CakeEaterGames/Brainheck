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
            /*
            Random rng = new Random();
            for (int t = 0; t <= 255; t++)
            {
                int val = t;
                int d3 = val % 10;
                val /= 10;
                int d2 = val % 10;
                val /= 10;
                int d1 = val % 10;
                

                lib.TestLists l = new lib.TestLists(true);

                if (d1 != 0)
                {
                    l.InputChars.Add((byte)('0' + d1));
                }
                if (d2 != 0 || d1!=0)
                {
                    l.InputChars.Add((byte)('0' + d2));
                }
                l.InputChars.Add((byte)('0' + d3));
                l.InputChars.Add(0);

                l.OutputChars.Add((byte)t);
                l.EndTape.Add((byte)t);


                Console.WriteLine(lib.GenerateTest(l.InputChars, l.OutputChars, l.StartingTape, l.EndTape, 0, 0, true, true, true));
            }
          
            Console.ReadLine(  );
  */

            LevelSelect.Init();
            SaveData.LoadFromFile();

            LevelSelect.Draw();

            Console.ReadLine();
        }
    }
}
