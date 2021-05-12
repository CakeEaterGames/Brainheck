using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainheck.legacy
{
    class Old_generators
    {
        public Old_generators()
        {
            /*
          Random rng = new Random();
          for (int t = 0; t < 10; t++)
          {
              lib.TestLists l = new lib.TestLists(true);

              lib.LES(l.StartingTape, 0, (byte)rng.Next(0, 256));
              lib.LES(l.StartingTape, 1, (byte)rng.Next(0, 256));

              lib.LES(l.EndTape, 0, (byte)(l.StartingTape[0] + l.StartingTape[1]));

              Console.WriteLine(  lib.GenerateTest(new List<byte>(), new List<byte>(), l.StartingTape, l.EndTape, 0, 0, false, true, true));
          }
         */

            Random rng = new Random();
            for (int t = 0; t < 10; t++)
            {
                lib.TestLists l = new lib.TestLists(true);

                lib.LES(l.StartingTape, 0, (byte)rng.Next(128, 256));

                lib.LES(l.EndTape, 3, l.StartingTape[0]);

                Console.WriteLine(lib.GenerateTest(new List<byte>(), new List<byte>(), l.StartingTape, l.EndTape, 0, 0, false, true, true));
            }


            /*
        Random rng = new Random();
        for (int t = 0; t <= 255; t++)
        {
            lib.TestLists l = new lib.TestLists(true);

            lib.LES(l.StartingTape, 0, (byte)t);

            lib.LES(l.EndTape, 0, (byte)t);
            lib.LES(l.EndTape, 1, (byte)t);


            Console.WriteLine(lib.GenerateTest(new List<byte>(), new List<byte>(), l.StartingTape, l.EndTape, 0, 0, false, true, true));
        }

        Console.ReadLine(  );
*/
        }
    }
}
