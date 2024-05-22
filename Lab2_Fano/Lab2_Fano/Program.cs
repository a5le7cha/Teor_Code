using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Lab2_Fano
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree treeCoder;
            Dictionary<char, double> dicF = new Dictionary<char, double>();
            List<char> alphabet = new List<char>();

            string text;
            using(var rd = new StreamReader("input.txt"))
            {
                text = rd.ReadToEnd();
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (!dicF.ContainsKey(text[i]))
                    dicF.Add(text[i], 1);
                else dicF[text[i]]++;

                if (!alphabet.Contains(text[i])) alphabet.Add(text[i]);
            }

            //упорядывачивание по значению
            dicF = dicF.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            //делаем вероятности в промежутке [0, 1]
            for(int i = 0; i<alphabet.Count; i++)
            {
                dicF[alphabet[i]] = dicF[alphabet[i]] / text.Length;
            }

            string symbols = "";
            foreach(var item in dicF)
            {
                symbols += item.Key;
            }

            treeCoder = new Tree(symbols, dicF);

            Dictionary<char, string> codes = new Dictionary<char, string>();
            treeCoder.Coding(treeCoder._root, "", codes);

            string codeText = "";
            for (int i = 0; i < text.Length; i++)
            {
                codeText += codes[text[i]];
            }

            Console.WriteLine("Кодовый словарь\n");
            foreach (var item in codes)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }

            Console.WriteLine($"\nКоэффициент сжатия:");
            Console.WriteLine($"{treeCoder.Ratio(text, codeText, codes)}\n");

            Console.WriteLine("Декодированный текст:");
            Console.WriteLine($"{treeCoder.Decoding(codeText, codes)}");

            Console.ReadKey();
        }
    }
}