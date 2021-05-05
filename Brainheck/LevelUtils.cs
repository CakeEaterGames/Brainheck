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
            LevelData.Add(LevelName.tut3, new LevelStruct(LevelName.tut3, "t3", "Tutorial 3"));
            LevelData.Add(LevelName.tut4, new LevelStruct(LevelName.tut4, "t4", "Tutorial 4"));

            LevelData.Add(LevelName.extut1, new LevelStruct(LevelName.extut1, "ext1", "Repetitions"));
            LevelData.Add(LevelName.extut2, new LevelStruct(LevelName.extut2, "ext2", "Functions"));
            LevelData.Add(LevelName.extut3, new LevelStruct(LevelName.extut3, "ext3", "Arguments"));
         
            LevelData.Add(LevelName.Move, new LevelStruct(LevelName.Move, "1", "Move"));

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

            UnlockConditions.Add(LevelName.extut1, new LevelName[] { LevelName.tut4 });
            UnlockConditions.Add(LevelName.extut2, new LevelName[] { LevelName.extut1 });
            UnlockConditions.Add(LevelName.extut3, new LevelName[] { LevelName.extut2 });
            UnlockConditions.Add(LevelName.extut4, new LevelName[] { LevelName.extut3 });


            UnlockConditions.Add(LevelName.Move, new LevelName[] { LevelName.tut4 });

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
     
    }

}
