using System;
using System.Collections.Generic;


namespace Lab6_Hemming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите двоичную последовательность: ");
            string BinPosl = Console.ReadLine();
            int[] _BinNumber = new int[BinPosl.Length];
            for(int i = 0; i<BinPosl.Length; i++)
            {
                if((Convert.ToInt32(BinPosl[i]) - 48) == 0 || (Convert.ToInt32(BinPosl[i]) - 48) == 1)
                    _BinNumber[i] = Convert.ToInt32(BinPosl[i]) - 48;
                else Console.WriteLine("Error input");
            }

            Console.WriteLine("Закодированная последовательность кодом хемминга: ");
            List<int> Code = Codirovanie(_BinNumber);

            foreach (var item in Code)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
            Console.WriteLine("Напишите номер позиции, в которой надо поменять бит на противоположный,\nнумерация начинается с 1: ");

            int NumError = int.Parse(Console.ReadLine());
            
            switch(Code[NumError - 1])
            {
                case 0:
                    Code[NumError - 1] = 1;
                    break;
                case 1:
                    Code[NumError - 1] = 0;
                    break;
            }

            Console.WriteLine("\nЗакодированное слово с ошибкой: ");
            foreach (var item in Code)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            Correcting(Code);

            Console.WriteLine("\nИсправленное слово: \n");
            foreach (var item in Code)
            {
                Console.Write($"{item} ");
            }

            Console.ReadKey();
        }

        static List<int> Codirovanie(int[] bin)
        {
            List<int> listCode = new List<int>();

            int j = 1;
            int i = 0;
            int k = 0;
            int two = 2;

            //встраивание контрльных бит
            while (i != bin.Length)
            {
                if(j != Math.Pow(two, k))
                {
                    listCode.Add(bin[i]);
                    i++;
                }
                else
                {
                    listCode.Add(0);
                    k++;
                }

                j++;
            }


            foreach (var item in listCode)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine("\nНажмите, чтобы посчитать значение контольных бит");
            Console.ReadKey();

            //Подсчет контрольных бит
            i = 1;
            int t;
            
            while (i <= listCode.Count)
            {
                j = i;
                while (j <= listCode.Count)
                {
                    t = j;

                    while(t<=listCode.Count && t<j+i)
                    {
                        listCode[i - 1] = (listCode[i - 1] + listCode[t - 1]) % 2;
                        t++;
                    }
                    j = t + i;
                }
                
                i *= 2;
            }


            return listCode;
        }

        static void Correcting(List<int> CodeError)
        {
            List<int> CopyCode = new List<int>();

            int i = 1;
            int t, j;
            int sum = 0, SumItog = 0;
            int two = 1;

            while (i <= CodeError.Count)
            {
                j = i;
                //sum = CodeError[i - 1];

                while (j <= CodeError.Count)
                {
                    t = j;

                    while (t <= CodeError.Count && t < j + i)
                    {
                        //sum = (CopyCode[i - 1] + CopyCode[t - 1]) % 2;
                        //CopyCode[i - 1] = sum;

                        sum = (sum + CodeError[t - 1]) % 2;
                        t++;
                    }
                    j = t + i;
                }

                SumItog += sum * i;
                sum = 0;
                i *= 2;
            }

            if(SumItog == 0) Console.WriteLine("Ошибок нет");
            else Console.WriteLine($"Ошибка в {SumItog} позиции");

            switch(CodeError[SumItog - 1])
            {
                case 0:
                    CodeError[SumItog - 1] = 1;
                    break;
                case 1:
                    CodeError[SumItog - 1] = 0;
                    break;
            }
        }
    }
}