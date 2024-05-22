using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Lab1_Haffman
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<char, int> FDic = new Dictionary<char, int>();

            string text;

            using(var rd = new StreamReader("WarAndWorld.txt"))
            {
                text = rd.ReadToEnd();
            }

            for(int i = 0; i<text.Length; i++)
            {
                if (FDic.ContainsKey(text[i]))
                    FDic[text[i]]++;
                else
                {
                    if ((int)text[i] == 13 || (int)text[i] == 10) continue;
                    else FDic.Add(text[i], 1);
                }
            }

            List<KeyValuePair<char, int>> listF = new List<KeyValuePair<char, int>>();
            foreach(var item in FDic)
            {
                listF.Add(new KeyValuePair<char, int>(item.Key, item.Value));
            }

            listF.Sort(
                delegate (KeyValuePair<char, int> val1,
                KeyValuePair<char, int> val2)
                {
                    return val1.Value.CompareTo(val2.Value);
                }
               );
            //listF.Reverse();

            Tree tree = new Tree(listF);

            Dictionary<char, string> codes = new Dictionary<char, string>();
            codes = GetCodes(tree._root);

            Console.WriteLine("Кодовый словарь: \n");
            foreach (var item in codes)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }
            Console.ReadKey();

            string codeText = "";
            for (int i = 0; i < text.Length; i++)
            {
                codeText += codes[text[i]];
            }

            Console.WriteLine("Кодированный текст:\n");
            Console.WriteLine($"{codeText}\n");

            int ratio = Ratio(text, codeText, codes);
            Console.WriteLine($"\nКоэффициент сжатия = {ratio}\n");

            string decode_text = Decodes(codes, codeText);
            Console.WriteLine("Декодированный текст: \n");
            for(int i = 0; i<decode_text.Length; i++)
            {
                Console.Write($"{decode_text[i]}");
            }

            Console.ReadKey();
        }

        //коэффициент сжатия
        static int Ratio(string InputText, string code, Dictionary<char, string> dicF)
        {
            int DegRatio = 0;

            string substring = "";
            int i = 0;
            while (i < code.Length)
            {
                if (dicF.ContainsValue(substring))
                {
                    DegRatio += substring.Length;
                    substring = "";
                }
                else
                {
                    substring += code[i];
                    i++;
                }
            }

            return (InputText.Length * 8) / DegRatio;
        }

        //кодовый словарь
        static Dictionary<char, string> GetCodes(Node root)
        {
            var codes = new Dictionary<char, string>();
            Transition(root, "", codes);
            return codes;
        }

        //декодирование
        static string Decodes(Dictionary<char, string> dicF, string code)
        {
            string substring = "";
            string text = "";

            int idx = 0;
            while (true)
            {
                if (dicF.ContainsValue(substring))
                {
                    text += dicF.FirstOrDefault(x => x.Value == substring).Key;   
                    substring = "";
                }
                else
                {
                    if (idx == code.Length) break;
                    substring += code[idx];
                    idx++;
                }
            }

            return text;
        }

        private static void Transition(Node node, string code, Dictionary<char, string> codes)
        {
            if (node == null)
            {
                return;
            }

            if (node.Symbol != '\0')
            {
                codes[node.Symbol] = code;
            }

            Transition(node.Left, code + "0", codes);
            Transition(node.Right, code + "1", codes);
        }
    }
}