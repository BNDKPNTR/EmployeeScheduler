using System.Collections.Generic;
using System.Collections.Immutable;

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
        public List<FixedAssaignment> FixedAssignments { get; } = new List<FixedAssaignment>();
        public List<string> ContractIDs { get; set; } = new List<string>();

        public Person()
        {
                
        }
    }
}