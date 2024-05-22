using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5_LZ77
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите текст, который надо сжать: ");
            string input = Console.ReadLine();

            List<Mark> listMark = Coding(input);//неправильно в методе кодирования, странно кодирует
            double step = CompretionRatio(input, listMark);

            Console.WriteLine($"коэффициент сжатия: {step}");

            foreach(var item in listMark)
            {
                Console.WriteLine($"{item.Bias} {item.MatchLen} {item.Symbol}");
            }

            string res = Decoding(listMark);

            Console.WriteLine($"Декодированная строка: {res}");

            Console.ReadKey();
        }

        static List<Mark> Coding(string input)
        {
            string SearchBuffer = ""; //буффер поиска, в него заносятся буквы которые уже считали
            int count = 0; //счетчик, будет считать, сколько символов идем назад
            int len = 0; //считает, сколько символов совпало
            char NowSymbol; //символ, который смотрим сейчас

            List<char> DicCharidxLastOccurent = new List<char>();

            List<Mark> ListMarks = new List<Mark>();//список меток

            ListMarks.Add(new Mark() { Bias = 0, MatchLen = 0, Symbol = input[0] });
            SearchBuffer += input[0];

            DicCharidxLastOccurent.Add(input[0]);

            for(int i = 1; i<input.Length; i++)
            {
                NowSymbol = input[i];
                if (!DicCharidxLastOccurent.Contains(NowSymbol))
                {
                    DicCharidxLastOccurent.Add(NowSymbol);
                    SearchBuffer += NowSymbol;
                    ListMarks.Add(new Mark() { Bias = 0, MatchLen = 0, Symbol = NowSymbol });
                }
                else
                {
                    for (int j = SearchBuffer.Length - 1; j >= 0; j--)
                    {
                        if (SearchBuffer[j] != NowSymbol) count++;
                        else
                        {
                            count++;
                            string str = "";
                            while (i < input.Length && j<SearchBuffer.Length)
                            {
                                if (input[i] == SearchBuffer[j])
                                {
                                    len++;
                                    str += input[i];
                                }
                                else break;

                                if (!DicCharidxLastOccurent.Contains(input[i])) DicCharidxLastOccurent.Add(input[i]);

                                i++;
                                j++;
                            }

                            SearchBuffer += str;

                            if (i == input.Length)
                            {
                                ListMarks.Add(new Mark() { Bias = count, MatchLen = len, Symbol = '$' });
                                break;
                            }

                            NowSymbol = input[i];
                            if (!DicCharidxLastOccurent.Contains(input[i])) DicCharidxLastOccurent.Add(input[i]);
                            SearchBuffer += NowSymbol;
                            ListMarks.Add(new Mark() { Bias = count, MatchLen = len, Symbol = NowSymbol });
                            count = 0; len = 0;

                            break;
                        }
                    }
                }
            }

            return ListMarks;
        }

        static string Decoding(List<Mark> listMark)//надо подумать, что сюда передавать
        {
            string result = "";

            for(int i = 0; i<listMark.Count; i++)
            {
                if (listMark[i].Bias == 0) result += listMark[i].Symbol;
                else
                {
                    int idx = result.Length - listMark[i].Bias, k = 0;

                    while(k < listMark[i].MatchLen)
                    {
                        result += result[idx];
                        idx++;
                        k++;
                    }

                    result += listMark[i].Symbol;
                }
            }

            return result;
        }

        static double CompretionRatio(string input, List<Mark> listMark)
        {
            double Ratio = input.Length * 8;

            int maxBias = listMark[0].Bias;
            int maxMatchLen = listMark[0].MatchLen;

            for(int i = 1; i<listMark.Count-1; i++)
            {
                if (listMark[i].Bias > maxBias)
                    maxBias = listMark[i].Bias;

                if (listMark[i].MatchLen > maxMatchLen)
                    maxMatchLen = listMark[i].MatchLen;
            }

            int bin_lenMaxBias = Convert.ToString(maxBias, 2).Length;
            int bin_lenMaxMatchLen = Convert.ToString(maxMatchLen, 2).Length;

            return Ratio / ((bin_lenMaxBias + bin_lenMaxMatchLen + 8) * listMark.Count-1);
        }
    }
}