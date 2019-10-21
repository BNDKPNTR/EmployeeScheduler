using Google.Protobuf.WellKnownTypes;

namespace IPScheduler.Models
{
    public class SchedulingMaxSeq
    {
        public static readonly SchedulingMaxSeq Empty = new SchedulingMaxSeq(empty :true);
        public bool IsEmpty { get; }
        public ShiftType Shift { get; set; }
        public int MaxValue { get; set; }

        private SchedulingMaxSeq(bool empty)
        {
            IsEmpty = empty;
        }

        public SchedulingMaxSeq()
        {
            
        }
    }
}
