using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    class Level
    {
        public void LoadFromRes(string id)
        {
            var RawMeta = lib.ResText("Levels." + id + ".meta.txt");
            var RamTests = lib.ResText("Levels." + id + ".tests.txt");

            var m = RawMeta.Split('\n');
            Name = m[0];
            Task = m[1];

            TicksReq = int.Parse(m[2]);
            CharsReq = int.Parse(m[3]);
            MemoryReq = int.Parse(m[4]);

            var t = RamTests.Split('\n');

        }
        int TicksReq, CharsReq, MemoryReq;

        struct LevelTest
        {

        }

        List<byte> Memory = new List<byte>();
        int CellsOnScreen = 19 * 6;

        public string Task = "Task text here";
        public string Name = "Level name";
        public string Input = "";

        int indexY, pointerY, cellY, codeY;

        public void DrawLayout()
        {
            Console.Clear();
            Console.WriteLine("Level: ");
            Console.WriteLine("    " + Name);

            Console.WriteLine("Task:");
            Console.WriteLine("    " + Task);
            Console.WriteLine();


            Console.WriteLine("Test №");

            Console.WriteLine("Input: " + Input);
            Console.WriteLine();

            string s = new string('-', Console.BufferWidth - 1);
            CellsOnScreen = (Console.BufferWidth - 1) / 6;

            indexY = Console.CursorTop - 1;

            Console.WriteLine(s);

            for (int i = 0; i < CellsOnScreen; i++)
            {
                Console.Write("|     ");
            }
            Console.WriteLine("|");

            cellY = Console.CursorTop - 1;
            for (int i = 0; i < CellsOnScreen; i++)
            {
                Console.Write("|     ");
            }
            Console.WriteLine("|");
            Console.WriteLine(s);

            pointerY = Console.CursorTop;

            Console.WriteLine();


            ControlPrompt();
            //Draw a layout
            //  Tittle, test number, input, empty memory, keys, place for code
            Console.WriteLine();
            codeY = Console.CursorTop;
            Console.WriteLine(Code);


            RewriteIndexes();
            RewritePointer();
            RewriteCells();
        }
        public void ControlPrompt()
        {
            Console.WriteLine("Enter: Run program; Space: Step; 1-9: Set speed; Escape: Exit; F: Open the level directory");
        }

        int lastPointerLocation = 0;
        int MemoryPointer = 0;
        public void RewriteIndexes()
        {
            Console.SetCursorPosition(0, indexY);
            int offset = (MemoryPointer / CellsOnScreen) * CellsOnScreen;
            for (int i = 0 + offset; i < CellsOnScreen + offset; i++)
            {
                int l = (i + "").Length;
                switch (l)
                {
                    case 1:
                        Console.Write("   {0}  ", i);
                        break;
                    case 2:
                        Console.Write("  {0}  ", i);
                        break;
                    case 3:
                        Console.Write("  {0} ", i);
                        break;
                    case 4:
                        Console.Write(" {0} ", i);
                        break;
                    case 5:
                        Console.Write(" {0}", i);
                        break;
                    default:
                        Console.Write("{0}", i % 10000);
                        break;
                }
            }

        }
        public void RewritePointer()
        {
            Console.SetCursorPosition((lastPointerLocation % CellsOnScreen) * 6, pointerY);
            Console.Write("      ");

            Console.SetCursorPosition((MemoryPointer % CellsOnScreen) * 6, pointerY);
            Console.Write("   {0}  ", "^");
            lastPointerLocation = MemoryPointer;
        }



        public void RewriteCells()
        {
            Console.SetCursorPosition(0, cellY);
            int offset = (MemoryPointer / CellsOnScreen) * CellsOnScreen;
            for (int i = 0 + offset; i < CellsOnScreen + offset; i++)
            {
                int v = 0;
                if (Memory.Count > i)
                {
                    v = Memory[i];
                }
                Console.SetCursorPosition((i % CellsOnScreen) * 6 + 2, cellY);

                if (v == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                int l = (v + "").Length;
                switch (l)
                {
                    case 1:
                        Console.Write(" {0} ", v);
                        break;
                    case 2:
                        Console.Write(" {0}", v);
                        break;
                    case 3:
                        Console.Write("{0}", v);
                        break;
                }
                Console.SetCursorPosition((i % CellsOnScreen) * 6 + 3, cellY + 1);
                if (v >= 32 && v <= 126)
                {
                    switch ((char)v)
                    {
                        case '\n':
                            Console.Write("\\n");
                            break;
                        default:
                            Console.Write(" " + (char)v);
                            break;
                    }
                }


                Console.ResetColor();

            }
        }

        Brainfuck bf;
        public string Code = "";
        int lastCodeLocation = 0;
        int CodePointer = 0;
        public void RewriteCodePos()
        {
            if (bf.Code.Length > CodePointer)
            {
                Console.ResetColor();
                Console.SetCursorPosition(lastCodeLocation % Console.BufferWidth, lastCodeLocation / Console.BufferWidth + codeY);
                Console.Write(Code[lastCodeLocation]);


                lastCodeLocation = CodePointer;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(CodePointer % Console.BufferWidth, CodePointer / Console.BufferWidth + codeY);
                Console.Write(bf.Code[CodePointer]);


            }
        }


        public void Run()
        {
            bf = new Brainfuck();
            bf.Init(Code, Memory);




            while (true)
            {

                Console.ReadKey(true);
                MemoryPointer = bf.MC;
                CodePointer = bf.PC;

                RewriteCells();
                RewriteIndexes();
                RewritePointer();
                RewriteCodePos();

                bf.Iterate();



            }


        }


    }



}

/* 
 TODO
 Load code from file
 Load level data from file
 Errors
 Debug (!)
 Sounds
 Load level tests
 Level input/output
 */
