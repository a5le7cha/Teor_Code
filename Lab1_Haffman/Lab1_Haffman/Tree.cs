using System.Collections.Generic;

namespace Lab1_Haffman
{
    class Tree
    {
        public Node _root;
        public Tree(List<KeyValuePair<char, int>> listF) 
        {
            var nodes = new List<Node>();
            foreach (var kvp in listF)
            {
                nodes.Add(new Node { Symbol = kvp.Key, Frequency = kvp.Value });
            }

            while (nodes.Count > 1)
            {
                nodes.Sort((x, y) => x.Frequency.CompareTo(y.Frequency));

                var left = nodes[0];
                var right = nodes[1];
                var parent = new Node { Frequency = left.Frequency + right.Frequency, Left = left, Right = right };

                nodes.Remove(left);
                nodes.Remove(right);
                nodes.Add(parent);
            }

            _root = nodes[0];
        }
    }
}