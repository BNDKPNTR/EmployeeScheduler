using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Person
    {
        public string Id { get; }
        public State State { get; }
        public bool[] Availabilities { get; }
        public bool[] ShiftOffRequests { get; set; }
        public bool[] ShiftOnRequests { get; set; }
        public Dictionary<int, Assignment> Assignments { get; }

        public Person(string id, State state, bool[] availabilities, bool[] shiftOffRequests, bool[] shiftOnRequests)
        {
            Id = id;
            State = state ?? throw new ArgumentNullException(nameof(state));
            Availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
            ShiftOffRequests = shiftOffRequests ?? throw new ArgumentNullException(nameof(shiftOffRequests));
            ShiftOnRequests = shiftOnRequests ?? throw new ArgumentNullException(nameof(shiftOnRequests));

            Assignments = new Dictionary<int, Assignment>();
        }

        public override string ToString() => Id;
    }
}
