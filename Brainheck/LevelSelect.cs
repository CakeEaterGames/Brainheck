using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brainheck
{
    public static class LevelSelect
    {

        public static void Init()
        {
            PrepareUnlocks();
            PrepareLevelTitles();
        }


        public static void Draw()
        {
            while (true)
            {
                Console.Clear();

                LevelGroup("Getting started");
                LevelTitle(LevelName.Intro);
                LevelTitle(LevelName.tut1);
                LevelTitle(LevelName.tut2);
                LevelTitle(LevelName.tut3);
                LevelTitle(LevelName.tut4);

                if (IsUnlocked(LevelName.extut1))
                {
                    LevelGroup("Extra tutorials");
                }
                LevelTitle(LevelName.extut1);
                LevelTitle(LevelName.extut2);
                LevelTitle(LevelName.extut3);


                if (IsUnlocked(LevelName.Move))
                {
                    LevelGroup("Basics");
                }
                LevelTitle(LevelName.Move);
                LevelTitle(LevelName.Clone);
                LevelTitle(LevelName.Set5);

                if (IsUnlocked(LevelName.AtoB))
                {
                    LevelGroup("ASCII");
                }
                LevelTitle(LevelName.AtoB);
                LevelTitle(LevelName.AtoB2);

                Console.WriteLine();
                LevelSelectPrompt();

                var inp = Console.ReadLine();



                if (LevelIds.ContainsKey(inp))
                {
                    var n = LevelData[LevelIds[inp]].Name.ToString();

                    switch (n)
                    {
                        case "Intro":
                            Dialog.FromRes("Dialogs.intro.txt");
                            SaveData.Set("Intro" + (LevelStat.IsSolved.ToString()), "true");
                            break;
                        default:
                            Level l = new Level(n);
                            l.Loop();
                            break;
                    }


                }
                else
                {
                    Console.WriteLine("No level with this ID. Try again");
                    Thread.Sleep(1000);

                }
            }
        }
        public static void LevelTitle(LevelName name)
        {
            if (IsUnlocked(name))
            {
                var s = LevelData[name];
                LevelTitle(
                    s.Title,
                    SaveData.GetBool(name.ToString() + LevelStat.IsSolved.ToString()),
                    SaveData.GetBool(name.ToString() + LevelStat.IsFast.ToString()),
                    SaveData.GetBool(name.ToString() + LevelStat.IsSmall.ToString()),
                    SaveData.GetBool(name.ToString() + LevelStat.IsMemory.ToString()),
                    s.Id,
                    s.Color
                );
            }
        }
        public static void LevelTitle(string name, bool isDone, bool isFast, bool isShort, bool isMemory, string code, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(code);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

            Console.Write("[");

            if (isDone)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("+");

            }
            else
            {
                Console.Write(" ");
            }

            if (isDone)
            {

         
            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (isFast) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (isShort) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (isMemory) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");

            Console.ForegroundColor = color;
            Console.Write(name);
            Console.WriteLine();
        }
        public static void LevelSelectPrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("To select a level type its ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("id ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("and press Enter");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LevelGroup(string name)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine(" " + name);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }








        public struct LevelStruct
        {
            public LevelName Name;
            public string Id;
            public string Title;
            public ConsoleColor Color;

            public LevelStruct(LevelName name, string id, string title, ConsoleColor color = ConsoleColor.White)
            {
                Name = name;
                Id = id;
                Title = title;
                Color = color;
            }
        }

        public static Dictionary<LevelName, LevelStruct> LevelData = new Dictionary<LevelName, LevelStruct>();
        public static Dictionary<string, LevelName> LevelIds = new Dictionary<string, LevelName>();
        public static void PrepareLevelTitles()
        {
            LevelData.Add(LevelName.Intro, new LevelStruct(LevelName.Intro, "0", "Introduction"));
            LevelData.Add(LevelName.tut1, new LevelStruct(LevelName.tut1, "t1", "Tutorial 1"));
            LevelData.Add(LevelName.tut2, new LevelStruct(LevelName.tut2, "t2", "Tutorial 2"));
            LevelData.Add(LevelName.tut3, new LevelStruct(LevelName.tut3, "t3", "Tutorial 3"));
            LevelData.Add(LevelName.tut4, new LevelStruct(LevelName.tut4, "t4", "Tutorial 4"));

            LevelData.Add(LevelName.extut1, new LevelStruct(LevelName.extut1, "ext1", "Repetitions"));
            LevelData.Add(LevelName.extut2, new LevelStruct(LevelName.extut2, "ext2", "Functions"));
            LevelData.Add(LevelName.extut3, new LevelStruct(LevelName.extut3, "ext3", "Arguments"));

            LevelData.Add(LevelName.Move, new LevelStruct(LevelName.Move, "b1", "Move"));
            LevelData.Add(LevelName.Clone, new LevelStruct(LevelName.Clone, "b2", "Clone"));
            LevelData.Add(LevelName.Set5, new LevelStruct(LevelName.Set5, "b3", "Set 5"));
            LevelData.Add(LevelName.AtoB, new LevelStruct(LevelName.AtoB, "a1", "Ascii to Byte"));
            LevelData.Add(LevelName.AtoB2, new LevelStruct(LevelName.AtoB2, "a12", "Ascii to Byte Extra",ConsoleColor.DarkYellow));

            foreach (var l in LevelData)
            {
                LevelIds.Add(l.Value.Id, l.Key);
            }
        }

        public static Dictionary<LevelName, LevelName[]> UnlockConditions = new Dictionary<LevelName, LevelName[]>();
        public static void PrepareUnlocks()
        {
            //UnlockConditions.Add(LevelName.tut1, new LevelName[] { });
            UnlockConditions.Add(LevelName.tut1, new LevelName[] { LevelName.Intro });
            UnlockConditions.Add(LevelName.tut2, new LevelName[] { LevelName.tut1 });
            UnlockConditions.Add(LevelName.tut3, new LevelName[] { LevelName.tut2 });
            UnlockConditions.Add(LevelName.tut4, new LevelName[] { LevelName.tut3 });

            UnlockConditions.Add(LevelName.extut1, new LevelName[] { LevelName.tut4 });
            UnlockConditions.Add(LevelName.extut2, new LevelName[] { LevelName.extut1, LevelName.Move });
            UnlockConditions.Add(LevelName.extut3, new LevelName[] { LevelName.extut2, LevelName.Move });

            UnlockConditions.Add(LevelName.Move, new LevelName[] { LevelName.tut4 });
            UnlockConditions.Add(LevelName.Clone, new LevelName[] { LevelName.tut4 });
            UnlockConditions.Add(LevelName.Set5, new LevelName[] { LevelName.tut4 });

            UnlockConditions.Add(LevelName.AtoB, new LevelName[] { LevelName.tut4 });
            UnlockConditions.Add(LevelName.AtoB2, new LevelName[] { LevelName.AtoB });

        }
        public static bool IsUnlocked(LevelName lvl)
        {
            if (!UnlockConditions.ContainsKey(lvl)) return true;

            foreach (var c in UnlockConditions[lvl])
            {
                if (!SaveData.GetBool(c.ToString() + LevelStat.IsSolved.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

    }


    public enum LevelStat
    {
        Unlocked,
        IsSolved,
        IsFast,
        IsSmall,
        IsMemory,
        Size,
        Steps,
        Memory,
    }

    public enum LevelName
    {
        Intro,

        tut1,
        tut2,
        tut3,
        tut4,

        extut1,
        extut2,
        extut3,
        extut4,

        Move,
        Clone,
        Set5,
        AtoB,
        AtoB2,
    }



}
