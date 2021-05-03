using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainheck
{
    using System;

    public class LevelUtils
    {
        public static void Init()
        {
            PrepareUnlocks();
            PrepareLevelTitles();
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


            foreach (var l in LevelData)
            {
                LevelIds.Add(l.Value.Id,l.Key);
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
            UnlockConditions.Add(LevelName.tut5, new LevelName[] { LevelName.tut4 });

            UnlockConditions.Add(LevelName.ClearCell, new LevelName[] { LevelName.tut5 });
            UnlockConditions.Add(LevelName.ClearCells, new LevelName[] { LevelName.ClearCell });

            UnlockConditions.Add(LevelName.MoveCell, new LevelName[] { LevelName.ClearCell });

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
        tut5,

        ClearCell,
        ClearCells,
        MoveCell,
    }

}
