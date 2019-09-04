using Google.OrTools.LinearSolver;

namespace IPScheduler.Models
{
    public class ShiftOffRequest
    {
        public int Day { get; set; }
        public ShiftType Type { get; set; }
        public Variable ShiftOffRrequestVariable { get; set; }
    }
}