using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab3_AriphmeticCod
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "";
            Dictionary<char, double> Frequancy = new Dictionary<char, double>();
            List<KeyValuePair<double, double>> ListOfBorder = new List<KeyValuePair<double, double>>();
            List<char> AllSymbols = new List<char>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                s = sr.ReadToEnd();
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (Frequancy.ContainsKey(s[i]))
                {
                    Frequancy[s[i]]++;
                }
                else
                {
                    Frequancy.Add(s[i], 1);
                    AllSymbols.Add(s[i]);
                }
            }
            int n = s.Length;
            for (int i = 0; i < AllSymbols.Count; i++)
            {
                Frequancy[AllSymbols[i]] = Frequancy[AllSymbols[i]] / n;
            }
            Frequancy = Frequancy.OrderBy(f => f.Key).ToDictionary(f => f.Key, f => f.Value);
            AllSymbols.Sort();
            KeyValuePair<double, double> border = new KeyValuePair<double, double>(0, 1);//Границы отрезка
            List<KeyValuePair<double, double>> SubBorders = new List<KeyValuePair<double, double>>();//разбиение отрезка на подотрезки
            int cnt = 0;
            double LeftSubBorder = 0;
            while (cnt != n)
            {
                SubBorders.Clear();
                double RightSubBOrder = border.Key;
                double Segment = border.Value - border.Key;
                LeftSubBorder = border.Key;
                for (int i = 0; i < AllSymbols.Count(); i++)
                {
                    RightSubBOrder += Segment * Frequancy[AllSymbols[i]];
                    SubBorders.Add(new KeyValuePair<double, double>(LeftSubBorder, RightSubBOrder));
                    LeftSubBorder = RightSubBOrder;
                }
                int index = AllSymbols.IndexOf(s[cnt]);//берем сегмент соответствующий индексу буквы
                ListOfBorder.Add(SubBorders[index]);//Список всех нужных нам границ для закодирования
                border = SubBorders[index];
                cnt++;
            }
            KeyValuePair<double, double> Answer = ListOfBorder.Last();
            //Console.WriteLine(Answer.Key + " " + Answer.Value);
            double Ans = Answer.Key;
            double AnsForRound = Answer.Key;
            bool flag = true;
            int c = 1;
            while (flag)
            {
                Ans = Math.Round(Answer.Key, c);
                if (Ans < Answer.Key)
                {
                    Ans += Math.Pow(10, -c);
                    if (Ans > Answer.Key && Ans < Answer.Value)
                    {
                        break;
                    }
                    else
                    {
                        Ans -= Math.Pow(10, -c);
                    }
                    c++;
                }
                else if (Ans > Answer.Value)
                {
                    c++;
                }
                else
                {
                    break;
                }
            }
            int EncodedMessage = (int)(Ans * Math.Pow(10, c));//Убираем нецелую часть
            string BinEncMes = Convert.ToString(EncodedMessage, 2);//Перевод в Двоичную
            Console.WriteLine("Закодированное сообщение:");
            Console.WriteLine(BinEncMes);
            Console.WriteLine($"K= {(double)s.Length * 8 / (double)BinEncMes.Length}");
            Console.WriteLine("Декодированный: ");

            int ENC = 0;
            int Decoded = Convert.ToInt32(BinEncMes, 2);//из двоичной
            string ss = Convert.ToInt32(BinEncMes, 2).ToString(); //Число знаков
            ENC = ss.Length;                                     // в числе
            double Dec = Decoded * Math.Pow(10, -ENC);
            border = new KeyValuePair<double, double>(0, 1);
            SubBorders = new List<KeyValuePair<double, double>>();
            string DecodedString = "";
            for (int i = 0; i < n; i++)
            {
                SubBorders.Clear();
                double RightSubBorder = border.Key;
                double Segment = border.Value - border.Key;
                LeftSubBorder = border.Key;
                for (int j = 0; j < AllSymbols.Count(); j++)
                {
                    RightSubBorder += Segment * Frequancy[AllSymbols[j]];
                    SubBorders.Add(new KeyValuePair<double, double>(LeftSubBorder, RightSubBorder));
                    LeftSubBorder = RightSubBorder;
                    if (RightSubBorder > Dec)
                    {
                        DecodedString += AllSymbols[j];
                        break;
                    }
                }
                border = SubBorders.Last();
            }
            Console.WriteLine(DecodedString);
            Console.ReadKey();
        }
    }
}