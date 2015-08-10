using System.Collections.Generic;
using System.Linq;

namespace Hediet.KeyboardMapper
{
    class ArrayComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(T[] obj)
        {
            return obj.Aggregate(0, (current, t) => current ^ t.GetHashCode());
        }
    }
}