using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2_Fano
{
    class Tree
    {
        public Node _root = new Node();
        public Tree(string symbols, Dictionary<char, double> dicF) 
        {
            _root.Key = symbols;
            _root.Value = 1;

            BuildTree(symbols, dicF, _root);
        }

        #region BuildTree
        private void BuildTree(string symboles, Dictionary<char, double> dicF, Node current)//нужно доделать
        {
            //нужно сортировать symboles по возрастанию частот

            string left = "", right = "";

            double frequencSum = 0; int index = 0;

            symboles = SortString(new StringBuilder(symboles), dicF);


            while(index != symboles.Length - 1)
            {
                if (frequencSum >= current.Value / 2) break;
                else
                {
                    frequencSum += dicF[symboles[index]];
                    right += symboles[index];
                    index++;
                }
            }

            //for(int i = 0; i<=symboles.Length; i++)
            //{
            //    if (frequencSum > current.Value / 2)
            //    {
            //        frequencSum = prevFreq;
            //        left = prev;
            //        index--;
            //        break;
            //    }

            //    if (i == symboles.Length) break;

            //    prevFreq = frequencSum;
            //    frequencSum += dicF[symboles[i]];

            //    prev = left;
            //    left += symboles[i];

            //    index++;
            //}

            while (index < symboles.Length)
            {
                left += symboles[index];
                index++;
            }

            current._left = new Node() { Key = left, Value = CalculateVer(left, dicF), Code = '0' };
            current._right = new Node() { Key = right, Value = CalculateVer(right, dicF), Code = '1' };

            if(left.Length != 1) BuildTree(left, dicF, current._left);
            if(right.Length != 1) BuildTree(right, dicF, current._right);
        }

        //калькулятор подсчета вероятностей
        double CalculateVer(string text, Dictionary<char, double> dicF)
        {
            double sum = 0;

            for (int i = 0; i < text.Length; i++)
            {
                sum += dicF[text[i]];
            }

            return sum;
        }
        #endregion

        //коэффициент сжатия
        public int Ratio(string InputText, string code, Dictionary<char, string> dicF)
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
        //кодирование 
        public void Coding(Node node, string code, Dictionary<char, string> codes)
        {
            if (node == null) return;

            if (node.Key[0] != '\0')
            {
                codes[node.Key[0]] = code;
            }

            Coding(node._left, code + '0', codes);
            Coding(node._right, code + '1', codes);
        }
        //декодирование
        public string Decoding(string codeText, Dictionary<char, string> codes)
        {
            string substring = "";
            string text = "";

            int idx = 0;
            while (true)
            {
                if (codes.ContainsValue(substring))
                {
                    text += codes.FirstOrDefault(x => x.Value == substring).Key;
                    substring = "";
                }
                else
                {
                    if (idx == codeText.Length) break;
                    substring += codeText[idx];
                    idx++;
                }
            }

            return text;
        }

        //сортировка строки символов
        static string SortString(StringBuilder input, Dictionary<char, double> dicF)
        {
            for(int i = 0; i<input.Length; i++)
            {
                for(int j = 0; j < input.Length - 1; j++)
                {
                    if (dicF[input[j]] > dicF[input[j + 1]])
                    {
                        var proxy = input[j];
                        input[j] = input[j + 1];
                        input[j+1] = proxy;
                    }
                }
            }

            return input.ToString();
        }
    }
}