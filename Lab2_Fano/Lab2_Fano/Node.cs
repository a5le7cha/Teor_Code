
namespace Lab2_Fano
{
    class Node
    {
        public Node() { }
        public Node _left, _right;
        public double Value { get; set; }//вероятность 
        public char Code { get; set; }//значение кода или '0' или '1'
        public string Key { get; set; }//символ char
    }
}