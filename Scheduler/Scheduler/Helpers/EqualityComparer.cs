using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    class EqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equalityComparerFunc;

        public EqualityComparer(Func<T, T, bool> equalityComparerFunc)
        {
            _equalityComparerFunc = equalityComparerFunc ?? throw new ArgumentNullException(nameof(equalityComparerFunc));
        }

        public bool Equals(T x, T y) => _equalityComparerFunc(x, y);

        public int GetHashCode(T obj) => 0;
    }

    static class EqualityComparer
    {
        public static EqualityComparer<T> Create<T>(Func<T, T, bool> equalityComparerFunc) => new EqualityComparer<T>(equalityComparerFunc);
    }
}
