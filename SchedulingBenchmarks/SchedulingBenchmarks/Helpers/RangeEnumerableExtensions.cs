using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    static class RangeEnumerableExtensions
    {
        public static List<Range> ExceptFrom(this IEnumerable<Range> ranges, Range from) => from.Except(ranges);

        public static List<Range> Normalize(this IEnumerable<Range> ranges) => Range.Normalize(ranges);

        public static IEnumerable<Range> IntersectWith(this IEnumerable<Range> ranges, Range with) => with.Intersect(ranges);
    }
}
