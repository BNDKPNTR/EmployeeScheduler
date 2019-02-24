using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equalsFunc;
        private readonly Func<T, int> _getHashCodeFunc;

        public EqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> getHashCodeFunc)
        {
            _equalsFunc = equalsFunc ?? throw new ArgumentNullException(nameof(equalsFunc));
            _getHashCodeFunc = getHashCodeFunc;
        }

        public static EqualityComparer<T> Create(Func<T, T, bool> equalsFunc, Func<T, int> getHashCodeFunc) 
            => new EqualityComparer<T>(equalsFunc, getHashCodeFunc);

        public bool Equals(T x, T y) => _equalsFunc(x, y);

        public int GetHashCode(T obj) => _getHashCodeFunc(obj);
    }
}
