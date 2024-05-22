using System;
using System.Collections.Generic;

namespace Lab7_LineCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] CodingMatrix = { { 1, 0, 1, 1, 1 }, 
                                    { 0, 1, 0, 1, 1 } };

            int[,] TestMatrixTransposed = { { 1, 1, 1}, 
                                            { 0, 1, 1}, 
                                            { 1, 0, 0},
                                            { 0, 1, 0},
                                            { 0, 0, 1}};

            Console.WriteLine("Введите сообщение: ");
            string input = Console.ReadLine();

            int[] Input = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                Input[i] = Convert.ToInt32(input[i]) - 48;
            }

            int[,] res = Coding(Input, CodingMatrix);

            Console.WriteLine("Закодированное сообщение: ");
            for(int i = 0; i<res.GetLength(1); i++)
            {
                Console.Write($"{res[0, i]} ");
            }

            Console.WriteLine("\nНапишите номер позиции, где нужно совершить ошибку, нумерация начинается с 1, наберите 8, если не нужно совершать ошибку: ");
            int num = int.Parse(Console.ReadLine());

            switch (num)
            {
                case 1:
                    if (res[0, 0] == 1) res[0, 0] = 0;
                    else res[0, 0] = 1;
                    break;
                case 2:
                    if (res[0, 1] == 1) res[0, 1] = 0;
                    else res[0, 1] = 1;
                    break;
                case 3:
                    if (res[0, 2] == 1) res[0, 2] = 0;
                    else res[0, 2] = 1;
                    break;
                case 4:
                    if (res[0, 3] == 1) res[0, 3] = 0;
                    else res[0, 3] = 1;
                    break;
                case 5:
                    if (res[0, 4] == 1) res[0, 4] = 0;
                    else res[0, 4] = 1;
                    break;
                default:
                    break;
            }

            Console.WriteLine("Сообщение с ошибкой: ");
            for (int i = 0; i < res.GetLength(1); i++)
            {
                Console.Write($"{res[0, i]} ");
            }
            Console.WriteLine("\n");
            int[,] dec = Decoding(res, TestMatrixTransposed);

            Console.WriteLine("\nДекодированный вектор: ");
            for (int i = 0; i < dec.GetLength(1); i++)
            {
                Console.Write($"{dec[0, i]} ");
            }

            Console.ReadKey();
        }
        static int[,] ProductMatrix(int[,] a, int[,] b)
        {
            int[,] result = new int[a.GetLength(0), b.GetLength(1)];
            for (int rowa = 0; rowa < a.GetLength(0); rowa++)
            {
                for (int colb = 0; colb < b.GetLength(1); colb++)
                {
                    result[rowa, colb] = 0;
                    for (int k = 0; k < a.GetLength(1); k++)
                    {
                        result[rowa, colb] = (result[rowa, colb] + a[rowa, k] * b[k, colb]) % 2;
                    }
                }
            }
            return result;
        }

        static int[,] Coding(int[] Input, int[,] CodMatrix)
        {
            int[,] matrixInpuut = new int[1, Input.Length];

            for (int i = 0; i< Input.Length; i++)
            {
                matrixInpuut[0, i] = Input[i];
            }

            return ProductMatrix(matrixInpuut, CodMatrix);
        }

        static int[,] Decoding(int[,] code, int[,] TestMatrixTrans)
        {
            //смежные классы:
            int[,] A0 = new int[4, 5] { { 0, 0, 0, 0, 0} ,
                                        { 1, 0, 1, 1, 1 } ,
                                        { 0, 1, 0, 1, 1 } ,
                                        { 1, 1, 1, 0, 0 } };

            List<int[,]> RelatedСlass = new List<int[,]>();
            int[,] vectors = new int[7, 5]
            {
                {1,0,0,0,0 },
                {0,1,0,0,0 },
                {0,0,1,0,0 },
                {0,0,0,1,0 },
                {0,0,0,0,1 },
                {1,0,0,0,1 },
                {1,0,0,1,0 },
            };

            for (int i = 0; i < 7; i++)
            {
                int[,] Ai = new int[4, 5];
                for(int j = 0; j<4; j++)
                {
                    for(int r = 0; r<5; r++)
                    {
                        Ai[j, r] = (vectors[i, r] + A0[j, r]) % 2;
                    }
                }

                RelatedСlass.Add(Ai);
            }

            Console.WriteLine("Смежные классы не включая А0: ");
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int r = 0; r < 5; r++)
                    {
                        Console.Write($"{RelatedСlass[i][j,r]} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n");
            }

            int[,] syndrome = ProductMatrix(code, TestMatrixTrans);

            Console.WriteLine("Синдром: ");
            for(int i = 0; i<syndrome.GetLength(1); i++)
            {
                Console.Write($"{syndrome[0, i]} ");
            }

            int[,] syndromes = new int[7, 3]
            {
                {1,1,1 },
                {0,1,1 },
                {1,0,0 },
                {0,1,0 },
                {0,0,1 },
                {1,1,0 },
                {1,0,1 }
            };

            int schet = 0;
            for (int i = 0; i < syndrome.GetLength(1); i++)
            {
                if (syndrome[0, i] == 0)
                {
                    schet++;
                }
            }

            if(schet == syndrome.GetLength(1))
            {
                Console.WriteLine("Ошибок нет");
                return code;
            }

            bool flag = true;
            int index = 0;
            for(int i = 0; i<syndromes.GetLength(0); i++)
            {
                for (int j = 0; j < syndromes.GetLength(1); j++)
                {
                    if (syndromes[i, j] != syndrome[0, j])
                    {
                        flag = false;
                        break;
                    }
                    else flag = true;
                }

                if(flag)
                {
                    index = i;
                    break;
                }
            }

            int NumVector = 0, minW = 6;

            for (int j = 0; j < 4; j++)
            {
                int w = 0;
                for (int r = 0; r < 5; r++)
                {
                    w += RelatedСlass[index][j, r];
                }

                if (minW > w)
                {
                    minW = w;
                    NumVector = j;
                }
            }

            int[,] result = new int[1, 5];

            for(int i = 0; i<code.GetLength(1); i++)
            {
                result[0, i] = code[0, i] - RelatedСlass[index][NumVector, i];
            }

            for (int i = 0; i < result.GetLength(1); i++)
            {
                if (result[0, i] < 0) result[0, i] = 2 + result[0, i];
            }

            return result;
        }
    }
}