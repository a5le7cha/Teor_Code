namespace Lab5_LZ77
{
    class Mark
    {
        public  static int maxBia = 0, maxMatchLen = 0;

        public char Symbol { get; set; } //символ, которые считали последним
        public int MatchLen { get; set; }//сколько символов совпало
        public int Bias { get; set; }//на сколько символов сместились назад

        void Max()
        {
            if (Bias > maxBia)
                maxBia = Bias;

            if (MatchLen > maxMatchLen)
                maxMatchLen = MatchLen;
        }
    }
}