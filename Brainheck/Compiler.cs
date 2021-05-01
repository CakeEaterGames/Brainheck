using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainheck
{

    public class Compiler
    {
        Dictionary<string, string> functions = new Dictionary<string, string>();
        public string Compile(string code)
        {
            string res = "";
            findFunctions(code);
            res = replaceFunctions(functions["main"]);
            res = unrawel(res);


            return res;
        }

        string unrawel(string code)
        {
            StringBuilder res = new StringBuilder();

            int i = 0;
            while (i < code.Length)
            {

                while (i < code.Length && !isNumber(code[i]))
                {
                    res.Append(code[i]);
                    i++;
                }
                string nums = "";
                while (i < code.Length && isNumber(code[i]))
                {
                    nums += code[i];
                    i++;
                }
                // Console.WriteLine(nums);
                if (i >= code.Length)
                {
                    break;
                }
                if (code[i] == '(')
                {
                    int st = i + 1;
                    int ed = st;
                    int depth = 1;
                    while (depth > 0)
                    {
                        if (code[ed] == ')')
                        {
                            depth--;
                        }
                        else
                        if (code[ed] == '(')
                        {
                            depth++;
                        }
                        ed++;
                    }
                    ed--;
                    //  Console.WriteLine("sss");
                    // Console.WriteLine(code.Substring(st, ed - st));


                    string un = unrawel(code.Substring(st, ed - st));
                    for (int j = 0; j < int.Parse(nums); j++)
                    {
                        res.Append(un);
                    }
                    i = ed + 1;
                }
                else
                {
                    res.Append(code[i], int.Parse(nums));
                    i++;
                }
            }
            return res.ToString();
        }
        bool isNumber(char c)
        {
            return (c >= '0' && c <= '9');
        }
        void findFunctions(string code)
        {
            int index = 0;

            index = code.IndexOf("#");
            while (index >= 0)
            {
                int st = code.IndexOf('{', index + 1);
                int ed = code.IndexOf('}', st);

                string name = RemoveSpaces(code.Substring(index + 1, st - index - 1));
                string fun = code.Substring(st + 1, ed - st - 1).Trim();
                //Console.WriteLine('"' + name + ':' + fun + '"');
                functions.Add(name, fun);

                index = code.IndexOf("#", index + 1);
            }
        }
        string replaceFunctions(string code)
        {
            bool found = true;
            while (found)
            {
                found = false;

                foreach (var f in functions)
                {
                    int index = code.IndexOf(f.Key + "(");
                    //Console.WriteLine("Searching " + f.Key);
                    while (index >= 0)
                    {
                        //  Console.WriteLine("found " + f.Key);


                        if (!(index > 0 && (isFuncChar(code[index - 1])) || (index + f.Key.Length < code.Length && isFuncChar(code[index + f.Key.Length]))))
                        {
                            int pst = code.IndexOf('(', index + f.Key.Length);
                            int ped = code.IndexOf(')', index + f.Key.Length);
                            string pars = code.Substring(pst + 1, ped - pst - 1);

                            string f2 = placeParams(f.Value, pars);

                            // Console.WriteLine(pars);
                            code = code.Substring(0, index) + f2 + code.Substring(ped + 1);
                        }
                        index = code.IndexOf(f.Key, index + 1);
                        found = true;
                        // Console.WriteLine(code);

                    }
                }
            }
            return code;
        }

        string placeParams(string code, string pars)
        {
            if (pars.Length > 0)
            {
                pars = RemoveSpaces(pars);
                var p = pars.Split(',');
                for (int i = p.Length - 1; i >= 0; i--)
                {
                    code = code.Replace("$" + i, p[i]);
                }
            }

            return code;
        }

        bool isFuncChar(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
            {
                return true;
            }
            return false;
        }

        string RemoveSpaces(string s)
        {
            return s.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        }

        int SkipComment(string code, int start)
        {
            return code.IndexOf("\n", start);
        }
    }

}
