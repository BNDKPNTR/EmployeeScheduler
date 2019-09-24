using System.Collections.Immutable;

namespace IPScheduler.Models
{
    public class SchedulingContract
    {
        public SchedulingMaxSeq MaxSeqs { get; set; }
        public ImmutableList<SchedulingMinSeq> MinSeqs { get; set; }
        public int? MinWork { get; set; }
        public int? MaxWork { get; set; }
        public string[] ValidShiftIDs  { get; set; }
    }
}