using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks
{
    public struct Range : IEnumerable<int>
    {
        public static readonly Range Empty = new Range();

        /// <summary>
        /// A tartomány inkluzív kezdete
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// A tartomány inkluzív vége
        /// </summary>
        public int End { get; }

        public int ExclusiveEnd => End + 1;

        public int Length => End - Start + 1;

        public Range(int? inclusiveStart = null, int? inclusiveEnd = null, int? length = null)
        {
            if (inclusiveStart.HasValue && inclusiveEnd.HasValue && length.HasValue)
            {
                throw new Exception($"Az {nameof(inclusiveStart)}, {nameof(inclusiveEnd)}, {nameof(length)} hármasból legfeljebb csak két paraméter adható meg");
            }

            if (length.HasValue && length < 1)
            {
                throw new Exception($"{nameof(length)} nem lehet kisebb, mint 1");
            }

            Start = inclusiveStart ?? inclusiveEnd.Value - length.Value + 1;
            End = inclusiveEnd ?? inclusiveStart.Value + length.Value - 1;

            if (End < Start)
            {
                throw new Exception($"{nameof(inclusiveEnd)} nagyobb, vagy egyenlő kell, hogy legyen, mint {nameof(inclusiveStart)}");
            }
        }

        public static Range Of(int? start = null, int? end = null, int? length = null) => new Range(start, end, length);

        public Range Mutate(int? start = null, int? end = null, int? length = null)
        {
            if (length.HasValue)
            {
                return start.HasValue || end.HasValue
                    ? new Range(
                        inclusiveStart: start,
                        inclusiveEnd: end,
                        length: length.Value)

                    : new Range(inclusiveStart: Start, length: length.Value);
            }

            return new Range(inclusiveStart: start ?? Start, inclusiveEnd: end ?? End);
        }

        public bool Contains(int element) => Start <= element && element <= End;

        public bool Contains(Range range) => Start <= range.Start && range.End <= End;

        public bool ContainsProperly(Range range) => Start < range.Start && range.End < End;

        public bool Overlaps(Range other) => other.Start < Start ? Start <= other.End : other.Start <= End;

        public Range Union(Range range) => new Range(Math.Min(Start, range.Start), Math.Max(End, range.End));

        public Range Intersect(Range range)
        {
            var start = Math.Max(Start, range.Start);
            var end = Math.Min(End, range.End);

            return end < start ? Empty : new Range(start, end);
        }

        public Range[] Except(Range other)
        {
            if (!Overlaps(other)) return new[] { this };
            if (ContainsProperly(other)) return this == other ? new[] { Empty } : new[] { new Range(Start, other.Start - 1), new Range(other.End + 1, End) };
            if (other.Contains(this)) return new[] { Empty };

            var intersection = Intersect(other);
            if (Start == intersection.Start) return new[] { new Range(intersection.End + 1, End) };
            return new[] { new Range(Start, intersection.Start - 1) };
        }

        public bool Equals(Range other) => Start == other.Start && End == other.End;

        public override bool Equals(object obj) => obj is Range other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                // Code copied from System.Tuple so it must be the best way to combine hash codes or at least a good one.
                return ((Start.GetHashCode() << 5) + Start.GetHashCode()) ^ End.GetHashCode();
            }
        }

        public static bool operator ==(Range left, Range right) => left.Equals(right);

        public static bool operator !=(Range left, Range right) => !left.Equals(right);

        public override string ToString() => $"{Start} - {End}";

        public IEnumerator<int> GetEnumerator() => Enumerable.Range(Start, Length).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<Range> Split(int size)
        {
            for (int i = Start; i <= End; i += size)
            {
                yield return new Range(i, Math.Min(i + size - 1, End));
            }
        }

        public IEnumerable<Range> Intersect(IEnumerable<Range> others)
        {
            var _this = this;
            return others.Where(r => _this.Overlaps(r)).Select(r => _this.Intersect(r));
        }

        public List<Range> Except(IEnumerable<Range> others)
        {
            var result = new List<Range> { this };

            foreach (var other in others)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    var excepts = result[i].Except(other);

                    if (excepts.Length == 1)
                    {
                        if (excepts[0] == Empty)
                        {
                            result.RemoveAt(i);
                            i--;
                            continue;
                        }

                        result[i] = excepts[0];
                        continue;
                    }

                    result[i] = excepts[0];
                    result.Insert(i + 1, excepts[1]);
                    i++;
                }
            }

            return result;
        }

        public static List<Range> Normalize(IEnumerable<Range> ranges)
        {
            var orderedRanges = ranges.OrderBy(r => r.Start).ToArray();
            var result = new List<Range>();

            if (orderedRanges.Length > 0)
            {
                result.Add(orderedRanges[0]);
            }

            for (int i = 1; i < orderedRanges.Length; i++)
            {
                var lastRange = result[result.Count - 1];

                if (lastRange.Overlaps(orderedRanges[i]))
                {
                    result[result.Count - 1] = lastRange.Union(orderedRanges[i]);
                }
                else
                {
                    result.Add(orderedRanges[i]);
                }
            }

            return result;
        }
    }
}
