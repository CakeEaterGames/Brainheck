using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    class Level
    {
        public string ID;
        public Level(string id)
        {
            ID = id;
        }
        public void LoadFromRes()
        {
            lib.DialogRes("Levels." + ID + ".IntroDialog.txt");
            Console.ResetColor();

            var RawMeta = lib.ResText("Levels." + ID + ".meta.txt");
            var RamTests = lib.ResText("Levels." + ID + ".tests.txt");

            var m = RawMeta.Split('\n');
            Name = m[0];
            Task = m[1];

            RequiredSteps = int.Parse(m[2]);
            RequiredSize = int.Parse(m[3]);
            RequiredMemory = int.Parse(m[4]);

            var tests = RamTests.Split('\n');

            foreach (var t in tests)
            {
                var r = t.Split(';');
                SolutionTest lt = new SolutionTest(r[0], r[1], r[2], r[3], r[4], r[5]);
                Tests.Add(lt);
            }

        }
        public List<SolutionTest> Tests = new List<SolutionTest>();
        public int CurrentTest = 0;
        public void SetTest(int index)
        {
            CurrentTest = index;
            Memory = new List<byte>(Tests[index].StartingTape);
            Input = new List<byte>(Tests[index].InputChars);
            ExpectedOutput = new List<byte>(Tests[index].OutputChars);
        }

 

        public struct SolutionTest
        {
            public List<byte> InputChars;
            public List<byte> OutputChars;
            public List<byte> StartingTape;
            public List<byte> EndTape;
            public int StartPos;
            public int EndPos;

            public bool ToCheckEndTape;
            public bool ToCheckEndOutput;
            public bool ToCheckEndPos;

            public SolutionTest(string inputChars, string outputChars, string startingTape, string endTape, string startPos, string endPos)
            {
                ToCheckEndTape = !endTape.Contains('*');
                ToCheckEndOutput = !outputChars.Contains('*');
                ToCheckEndPos = !endPos.Contains('*');

                InputChars = lib.StringToByteList(inputChars, ',');
                StartingTape = lib.StringToByteList(startingTape, ',');
                StartPos = int.Parse(startPos);

                EndTape = null;
                OutputChars = null;
                EndPos = 0;

                if (ToCheckEndTape)
                {
                    EndTape = lib.StringToByteList(endTape, ',');
                }
                if (ToCheckEndOutput)
                {
                    OutputChars = lib.StringToByteList(outputChars, ',');
                }
                if (ToCheckEndPos)
                {
                    EndPos = int.Parse(endPos);
                }            
            }
         


        }

        string EmptySolution = "#main\n{\n\n}";
        public void CreateFoulders()
        {
            string path = "Soulutions/" + ID;
            Directory.CreateDirectory(path);
            if (!File.Exists(path+"/CurrentSolution.txt"))
            {
                StreamWriter sw = new StreamWriter(path + "/CurrentSolution.txt");
                sw.Write(EmptySolution);
                sw.Flush();
                sw.Close();
            }
        }
        public void LoadSolutionFromFile()
        {
            string path = "Soulutions/" + ID;
            StreamReader sr = new StreamReader(path + "/CurrentSolution.txt");
            var highCode = sr.ReadToEnd();
            sr.Close();
            Compiler c = new Compiler();
            Code = c.Compile(highCode);
            Code = OptimiseSolution(Code);
        }

        List<byte> Memory = new List<byte>();
        List<byte> Input = new List<byte>();
        List<byte> ExpectedOutput = new List<byte>();
        int CellsOnScreen = 19 * 6;

        public string Task = "Task text here";
        public string Name = "Level name";

        int indexY, pointerY, cellY, codeY;

        public void DrawLayout()
        {
            Console.Clear();
            Console.WriteLine("Level: ");
            Console.WriteLine("    " + Name);

            Console.WriteLine("Task:");
            Console.WriteLine("    " + Task);
            Console.WriteLine();


            Console.WriteLine("Test №"+CurrentTest);

            Console.WriteLine("Input:  " + string.Join(", ", Input));
            Console.WriteLine();
            Console.WriteLine("Output: " + string.Join(", ", ExpectedOutput));
            Console.WriteLine();

            string s = new string('-', Console.BufferWidth - 1);
            CellsOnScreen = (Console.BufferWidth - 1) / 6;

            indexY = Console.CursorTop - 1;

            Console.WriteLine(s);

            for (int i = 0; i < CellsOnScreen; i++)
            {
                Console.Write("|     ");
            }

            if ((Console.BufferWidth - 1) % 6>=1) Console.Write("|");
            Console.WriteLine();
 

            cellY = Console.CursorTop - 1;
            for (int i = 0; i < CellsOnScreen; i++)
            {
                Console.Write("|     ");
            }

            if ((Console.BufferWidth - 1) % 6 >= 1) Console.Write("|");
            Console.WriteLine();
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
            Console.WriteLine("Enter: Run program;       Space: Step;       Escape: Exit;       F: Open the level directory");
        }

        int lastPointerLocation = 0;
        int MemoryPointer = 0;

        int LastMemoryPage = -1;
        int MemoryPage = 0;


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

        public void RewriteCell(int i)
        {
            
            Console.SetCursorPosition(0, cellY);

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

        public void RewriteCells()
        {
            Console.SetCursorPosition(0, cellY);
            int offset = (MemoryPointer / CellsOnScreen) * CellsOnScreen;
            for (int i = 0 + offset; i < CellsOnScreen + offset; i++)
            {
                RewriteCell(i);
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
                if (Code.Length> lastCodeLocation)
                {
                    Console.Write(Code[lastCodeLocation]);
                }
                else
                {
                    Console.Write(" ");
                }
            


                lastCodeLocation = CodePointer;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(CodePointer % Console.BufferWidth, CodePointer / Console.BufferWidth + codeY);
                if (bf.Code.Length > CodePointer)
                {
                    Console.Write(bf.Code[CodePointer]);
                }
                else
                {
                    Console.Write(" ");
                }

                Console.ResetColor();

            }
        }

        public enum RunSpeed
        {
            steps,
            breaks,
            full,
            cancel
        }
        public void Run(RunSpeed sp)
        {
           // LoadFromRes();
            SetTest(CurrentTest);
            LoadSolutionFromFile();
            DrawLayout();

            bf = new Brainfuck();
            bf.Init(Code, Memory, Input);

            
            while (bf.IsRunning)
            {
                MemoryPointer = bf.MC;
                CodePointer = bf.PC;

                //TODO: check if needed to rewrite
                if (bf.Command == '+' || bf.Command == '-')
                {
                    RewriteCell(MemoryPointer);
                }
                if (bf.Command == '>' || bf.Command == '<')
                {
                    RewritePointer();

                    MemoryPage = MemoryPointer / CellsOnScreen;
                    if (LastMemoryPage!=MemoryPage)
                    {
                        RewriteIndexes();
                    }
                    LastMemoryPage = MemoryPage;
                }

               
                RewriteCodePos();

                bf.Iterate();

                // bf.PlaySound();
                // Thread.Sleep(1);

                if (sp==RunSpeed.steps)
                {
                    var k = Console.ReadKey(true);
                    if (k.Key == ConsoleKey.Enter)
                    {
                        sp = RunSpeed.breaks;
                    }
                    if (k.Key == ConsoleKey.Escape)
                    {
                        sp = RunSpeed.cancel;
                        break;
                    }
                }
            }

            Console.Clear();

            if (sp != RunSpeed.cancel)
            {
                bool solved = VeryfySolution();

                if (solved)
                {
                    IsLevelSolved = true;
                }
            }

            MemoryPointer = 0;
            CodePointer = 0;
            Console.Clear();
            SetTest(CurrentTest);
            LoadSolutionFromFile();
            DrawLayout();
        }


        public string OptimiseSolution(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            foreach (var c in s)
            {
                switch (c)
                {
                    case '+':
                        if (sb[sb.Length-1]=='-')
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                    case '-':
                        if (sb[sb.Length - 1] == '+')
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                    case '>':
                        if (sb[sb.Length - 1] == '<')
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                    case '<':
                        if (sb[sb.Length - 1] == '>')
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                    case '.':
                        sb.Append(c);
                        break;
                    case ',':
                        sb.Append(c);
                        break;
                    case '[':
                        sb.Append(c);
                        break;
                    case ']':
                        sb.Append(c);
                        break;
                    default:
                        break;
                }
            }
            sb.Remove(0, 1);
            return sb.ToString();

        }

        public bool VeryfySolution()
        {
            Code = OptimiseSolution(Code);
            int CurSteps=0, CurSize = 0, CurMemory = 0;
            for (int ti = 0; ti < Tests.Count; ti++)
            {
                var test = Tests[ti];
                Brainfuck bf = new Brainfuck();
                bf.Init(Code,new List<byte>(test.StartingTape), new List<byte>(test.InputChars));
                bf.FastRun();

                bool valid = true;

                switch (bf.CurrentState)
                {
                    case Brainfuck.State.Noraml:
                        break;
                    case Brainfuck.State.Finished:
                        break;
                    case Brainfuck.State.OutOfBounds:
                        valid = false;
                        Console.WriteLine("Instruction Pointer got out of bounds");
                        break;
                    case Brainfuck.State.NoInput:
                        valid = false;
                        Console.WriteLine("Attempted to get input but input was empty");
                        break;
                    case Brainfuck.State.TooManySteps:
                        valid = false;
                        Console.WriteLine("The program ran for " + bf.MaxStepCount + " steps.");
                        Console.WriteLine("If this is intended, please change the MaxStepCount variable in cfg.ini");
                        break;
                    default:
                        break;
                }
            
                if (test.ToCheckEndOutput)
                {
                    var t1 = string.Join(", ", bf.Output);
                    var t2 = string.Join(", ", test.OutputChars);
                    if (t1!=t2)
                    {
                        Console.WriteLine("Expected output: " + t2);
                        Console.WriteLine("But got: " + t1);
                        Console.WriteLine();
                        valid = false;
                    }
                }
                if (test.ToCheckEndPos)
                {
                    if (test.EndPos != bf.MC)
                    {
                        valid = false;
                        Console.WriteLine("Expected eye location: " + test.EndPos);
                        Console.WriteLine("But got: " + bf.MC);
                        Console.WriteLine();
                    }
                }
                if (test.ToCheckEndTape)
                {
                    var t1 = string.Join(", ", bf.GetFinalTape());
                    var t2 = string.Join(", ", test.EndTape);
                    if (t1 != t2)
                    {
                        Console.WriteLine("Expected tape contents: " + t2);
                        Console.WriteLine("But got: " + t1);
                        Console.WriteLine();
                        valid = false;
                    }
                }

                if (!valid)
                {
                    CurrentTest = ti;
                    Console.WriteLine("Failed test "+ti);
                    Console.ReadKey(true);
                    SetTest(ti);
                    return false;
                }
                CurSteps += bf.StepCount;
                CurMemory += bf.UsedMemory;
            }
            CurrentSize = Code.Length;
            CurrentMemory = CurMemory / Tests.Count;
            CurrentSteps = CurSteps / Tests.Count;

            return true;
        }

        public int BestSteps, BestSize, BestMemory;
        public int CurrentSteps, CurrentSize, CurrentMemory;
        public int RequiredSteps, RequiredSize, RequiredMemory;

        public void CongratulationScreen()
        {
            Console.Clear();
            Console.WriteLine("Task Complete!");
            Console.WriteLine("Steps: " + CurrentSteps);
            Console.WriteLine("Size: " + CurrentSize);
            Console.WriteLine("Memory: " + CurrentMemory);

        }

        public bool IsLevelSolved = false;

        public void Loop()
        {
            Console.ResetColor();
            LoadFromRes();
            CreateFoulders();
            SetTest(0);
            LoadSolutionFromFile();
            DrawLayout();

            

            while (true)
            {
                bool repeat = true;
                bool toBreak = false;
                while (repeat)
                {
                    repeat = false;
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    switch (k.Key)
                    {
                        case ConsoleKey.Escape:
                            toBreak = true;
                            break;
                        case ConsoleKey.Enter:
                            Run(RunSpeed.breaks);
                            break;
                        case ConsoleKey.Spacebar:
                            Run(RunSpeed.steps);
                            break;
                        case ConsoleKey.F:
                            Process.Start(Directory.GetCurrentDirectory()+ "/Soulutions/" + ID);
                            break;
                        default:
                            repeat = true;
                            break;
                    }
                }

                if (IsLevelSolved)
                {
                    SaveData.Set(ID+(LevelStat.IsSolved.ToString()),"true");

                    if (CurrentSteps<=RequiredSteps)
                    {
                        SaveData.Set(ID + (LevelStat.IsFast.ToString()), "true");
                    }
                    if (CurrentSize <= RequiredSize)
                    {
                        SaveData.Set(ID + (LevelStat.IsSmall.ToString()), "true");
                    }
                    if (CurrentMemory <= RequiredMemory)
                    {
                        SaveData.Set(ID + (LevelStat.IsMemory.ToString()), "true");
                    }
                    CongratulationScreen();
                    Console.ReadKey(true);
                    lib.DialogRes("Levels." + ID + ".SolvedDialog.txt");
                    break;
                }

                if (toBreak) break;
            }
        }

    }

}

 /* 
 TODO
 Debug (!)
 Sounds
 cfg.ini
 */
