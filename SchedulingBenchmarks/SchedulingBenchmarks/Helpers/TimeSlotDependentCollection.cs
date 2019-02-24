using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    class TimeSlotDependentCollection<T>
    {
        private readonly List<IndexPair> _sparseIndexes;
        private readonly List<T> _elements;
        private int i;

        public T Current { get; private set; }

        public TimeSlotDependentCollection(params (Range range, T value)[] valueRanges)
        {
            i = 0;
            _sparseIndexes = new List<IndexPair>();
            _elements = new List<T>();

            for (int i = 0; i < valueRanges.Length; i++)
            {
                var valueRange = valueRanges[i];

                _elements.Add(valueRange.value);
                _sparseIndexes.Add(new IndexPair(valueRange.range.Start, _elements.Count - 1));
            }

            if (_elements.Count > 0)
            {
                Current = _elements[0];
            }
        }

        public void MoveNext(int timeSlot)
        {
            if (_sparseIndexes.Count == i + 1 || timeSlot < _sparseIndexes[i + 1].TimeSlot)
            {
                return;
            }

            Current = _elements[_sparseIndexes[++i].ElementIndex];
        }

        private struct IndexPair
        {
            public readonly int TimeSlot;
            public readonly int ElementIndex;

            public IndexPair(int timeSlot, int elementIndex)
            {
                TimeSlot = timeSlot;
                ElementIndex = elementIndex;
            }
        }
    }
}
