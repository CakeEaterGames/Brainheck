using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainheck
{
    static class SaveData
    {
        public static Dictionary<string, string> Var = new Dictionary<string, string>();
        public static void Set(string name, string val)
        {
            Var[name] = val;
            SaveToFile();
        }
        public static string GetString(string name)
        {
            if (Var.ContainsKey(name))
                return Var[name];
            return "";
        }
        public static int GetInt(string name)
        {
            return int.Parse(GetString(name));
        }
        public static bool GetBool(string name)
        {
            return GetString(name) == "true";
        }

        public static void SaveToFile()
        {
            StringBuilder res = new StringBuilder();

            foreach (var var in Var)
            {
                res.Append(var.Key);
                res.Append(":");
                res.Append(var.Value);
                res.Append(";");
            }

            StreamWriter sw = new StreamWriter("save");
            sw.Write(res.ToString());
            sw.Flush();
            sw.Close();

        }
        public static void LoadFromFile()
        {
            if (!File.Exists("save"))
            {
                StreamWriter sw = new StreamWriter("save");
                sw.Write("");
                sw.Flush();
                sw.Close();
            }

            Var = new Dictionary<string, string>();
            StreamReader sr = new StreamReader("save");
            var data = sr.ReadToEnd().Replace("\r", "").Split(';');
            foreach (var v in data)
            {
                var pair = v.Split(':');
                if (pair[0] != "")
                { 
                    Var[pair[0]] = pair[1];
                }
         
            }
            sr.Close();
        }


    }



}
