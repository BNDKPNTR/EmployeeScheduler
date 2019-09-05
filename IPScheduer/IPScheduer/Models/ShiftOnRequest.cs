using Google.OrTools.LinearSolver;

namespace IPScheduler.Models
{
    public class ShiftOnRequest
    {
        public int Day { get; set; }
        public ShiftType Type { get; set; }
        
        public Variable ShiftOnRrequestVariable { get; set; }
        public int Weight { get; set; }
    }
}