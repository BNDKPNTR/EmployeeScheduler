using IPScheduler.Common;

namespace IPScheduler.Models
{
    public class ShiftType
    {
        public string ID { get; set; }
        public Time StartTime { get; set; }
        public int DurationInMnutes { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }

        public bool IsSame(ShiftType other)
            => other.ID.Equals(this.ID);
    }
}