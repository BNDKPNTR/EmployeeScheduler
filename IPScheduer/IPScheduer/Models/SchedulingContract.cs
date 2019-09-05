using System.Collections.Immutable;

namespace IPScheduler.Models
{
    public class SchedulingContract
    {
        public ImmutableList<SchedulingMaxSeq> MaxSeqs { get; set; }
        public ImmutableList<SchedulingMinSeq> MinSeqs { get; set; }
    }
}