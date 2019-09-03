using System.Collections.Immutable;
using IPScheduler.Common;

namespace IPScheduler.Models
{
    public class SchedulingContract
    {
        public ImmutableList<SchedulingMaxSeq> MaxSeqs { get; set; }
        public ImmutableList<SchedulingMinSeq> MinSeqs { get; set; }
    }
}