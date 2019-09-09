using Google.OrTools.LinearSolver;

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
        public int MaxWeight { get; set; }
        public int MinWeight { get; set; }

        public Variable OverMax { get; set; }
        public Variable UnderMin { get; set; }
    }
}