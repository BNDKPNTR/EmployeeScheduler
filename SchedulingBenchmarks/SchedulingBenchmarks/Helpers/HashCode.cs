using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    public static class HashCode
    {
        public static int From<T1, T2>(T1 member1, T2 member2)
        {
            var hashCode = member1.GetHashCode();
            return Combine(hashCode, member2);
        }

        private static int Combine<T>(int hashCode, T member)
        {
            unchecked
            {
                return ((hashCode << 5) + hashCode) ^ member.GetHashCode();
            }
        }
    }
}
