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

        public struct TestLists
        {
            public List<byte> InputChars;
            public List<byte> OutputChars;
            public List<byte> StartingTape;
            public List<byte> EndTape;

            public TestLists(bool init)
            {
                InputChars = new List<byte>();
                OutputChars = new List<byte>();
                StartingTape = new List<byte>();
                EndTape = new List<byte>();
            }
        }
        public static string GenerateTest(List<byte> Input, List<byte> Output, List<byte> StartingMemory, List<byte> EndMemory, int StartingIndex, int EndIndex, bool toCheckOutput, bool toCheckMemory, bool toCheckIndex)
        {
            StringBuilder res = new StringBuilder();

            res.Append(String.Join(",", Input));
            res.Append("; ");
            if (toCheckOutput)
            {
                res.Append(String.Join(",", Output));
            }
            else
            {
                res.Append("*");
            }
            res.Append("; ");

            res.Append(String.Join(",", StartingMemory));
            res.Append("; ");
            if (toCheckMemory)
            {
                res.Append(String.Join(",", EndMemory));
            }
            else
            {
                res.Append("*");
            }
            res.Append("; ");

            res.Append(StartingIndex);
            res.Append("; ");
            if (toCheckMemory)
            {
                res.Append(EndIndex);
            }
            else
            {
                res.Append("*");
            }
            res.Append("; ");

            return res.ToString();
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

        /// <summary>
        /// ListExpandSet
        /// </summary>
        public static void LES(List<byte> list, int index, byte value)
        {
            while (list.Count<=index)
            {
                list.Add(0);
            }
            list[index] = value;
        }
 
    }
}
