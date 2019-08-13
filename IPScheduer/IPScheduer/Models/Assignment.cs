using Google.OrTools.LinearSolver;

namespace IPScheduler.Models
{
    public class Assignment
    {
        public int Index { get; set; }

        public Person Person { get; set; }
        public Shift Shift { get; set; }
        public Variable assigningGraphEdge { get; set; }

        public override string ToString() => $"{Person.Name}-{Shift.Name}";
    }
}