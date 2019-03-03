using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Shift
    {
        public int Index { get; }
        public string Id { get; set; }

        public Shift(int index, string id)
        {
            Index = index;
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
