namespace IPScheduler.Models
{
    public class Shift
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public ShiftType Type { get; set; }

        public int Day { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
    }
}