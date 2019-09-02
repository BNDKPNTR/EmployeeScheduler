using System.Collections.Generic;

namespace IPScheduler.Models
{
    public class Person
    {
        public Person(string id)
        {
            ID = id;
        }

        public string ID { get; set; }
        public int Index { get; set; }
        public object Name { get; set; }
        public Dictionary<int,ShiftOnRequest> ShiftOnRequests { get; } = new Dictionary<int, ShiftOnRequest>();
        public Dictionary<int, ShiftOffRequest> ShiftOffRequests { get; } = new Dictionary<int, ShiftOffRequest>();
        public List<FixedFreeDay> FixedFreeDays { get; } = new List<FixedFreeDay>();
        public Dictionary<int, FixedAssaignment> FixedAssignments { get; } = new Dictionary<int, FixedAssaignment>();
        public List<SchedulingContract> Contracts { get; } = new List<SchedulingContract>();

        public Person()
        {
                
        }
    }
}