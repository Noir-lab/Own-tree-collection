using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tree
{
    public class OwnComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null || y == null) return 0;

            return x.CompareTo(y);
        }
    }
}