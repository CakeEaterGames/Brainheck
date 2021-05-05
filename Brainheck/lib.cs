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
        public static void BeepTest()
        {
            while (true)
            {
                var d = int.Parse(Console.ReadLine());
                int step = 100;
                for (int i = 0; i < 30; i++)
                {
                    Console.Beep(d + step * i, 10);
                    Thread.Sleep(10);
                }
               
             

            }
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

        public static List<byte> StringToByteList(string S, char sep)
        {
            List<byte> res = new List<byte>();
            var parts = S.Split(sep);
            foreach (var s in parts)
            {
                if (s.Trim().Length!=0)
                {
                    res.Add(byte.Parse(s.Trim()));
                }
            }
            return res;
        }
 
    }
}
