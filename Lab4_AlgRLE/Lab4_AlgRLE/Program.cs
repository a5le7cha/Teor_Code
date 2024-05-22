using System;

namespace Lab4_AlgRLE
{
    #region Code_class
    public class CodeRLE
    {
        public string codeString;
        public int CorectedString;
        public CodeRLE(string str, int num)
        {
            this.codeString = str;
            this.CorectedString = num;
        }

        public override string ToString()
        {
            return String.Format($"({this.codeString}, {this.CorectedString})");
        }
    }
    #endregion
    class Program
    {
        public static CodeRLE Encoding(string input)
        {
            int message_len = input.Length;
            string[] array = new string[message_len];
            string EncodingMessage = "";

            array[0] = input;

            for (int i = 1; i < message_len; i++)
            {
                array[i] = array[i - 1];

                char ch;
                ch = array[i][message_len - 1];
                array[i] = array[i].Remove(message_len - 1, 1);
                array[i] = array[i].Insert(0, ch.ToString());
            }

            Array.Sort(array);

            for (int i = 0; i < array.Length; i++)
            {
                EncodingMessage += array[i][message_len - 1];
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == input)
                    return new CodeRLE(EncodingMessage, i + 1);
            }

            return null;
        }

        public static string Compression(string message, CodeRLE encodedMessage)
        {
            string RLEmessage = "";
            double K = message.Length * 8;
            int max_num = -1, cnt_of_pairs = 0;

            for (int i = 0; i < message.Length;)
            {
                char ch = encodedMessage.codeString[i++];
                int cnt = 1;
                RLEmessage += ch;
                while (i < message.Length && ch == encodedMessage.codeString[i])
                {
                    cnt++;
                    i++;
                }
                if (cnt > max_num) { max_num = cnt; }
                RLEmessage += cnt.ToString();
                cnt_of_pairs++;
            }

            K /= ((Convert.ToString(max_num, 2).Length + 8) * cnt_of_pairs + Convert.ToString(encodedMessage.CorectedString, 2).Length);
            Console.WriteLine($"Степень сжатия: {K}");

            return RLEmessage;
        }

        public static string Decoding(CodeRLE encodedMessage)
        {
            string Emessage = encodedMessage.codeString;
            string[] array = new string[Emessage.Length];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Emessage[i].ToString();
            }
            Array.Sort(array);

            while (array[0].Length < Emessage.Length)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = array[i].Insert(0, Emessage[i].ToString());
                }
                Array.Sort(array);
            }

            return array[encodedMessage.CorectedString - 1];
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Введите последовательность, которую надо закодировать:");
            string input = Console.ReadLine();

            CodeRLE encodedMessage = Encoding(input);

            Console.WriteLine($"Код: {Compression(input, encodedMessage)}");

            Console.WriteLine($"Расшифрованный тексе: {Decoding(encodedMessage)}");

            Console.ReadKey();
        }
    }
}