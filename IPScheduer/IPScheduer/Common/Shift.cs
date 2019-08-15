namespace IPScheduler.Common
{
    public class Shift
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string Type { get; set; }

        public int Day { get; set; }
        public ShiftPriority Priority { get; set; }
    }
}