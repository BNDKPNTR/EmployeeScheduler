using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Person
    {
        public int Id { get; }
        public string ExternalId { get; }
        public State State { get; set; }
        public WorkSchedule WorkSchedule { get; }
        public bool[] Availabilities { get; }
        public Dictionary<int, ShiftRequest> ShiftRequests { get; }
        public AssignmentsCollection Assignments { get; set; }

        public Person(int id, string externalId, State state, WorkSchedule workSchedule, bool[] availabilities, Dictionary<int, ShiftRequest> shiftRequests)
        {
            Id = id;
            ExternalId = externalId;
            State = state ?? throw new ArgumentNullException(nameof(state));
            WorkSchedule = workSchedule ?? throw new ArgumentNullException(nameof(workSchedule));
            Availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
            ShiftRequests = shiftRequests ?? throw new ArgumentNullException(nameof(shiftRequests));

            Assignments = new AssignmentsCollection();
        }

        public override string ToString() => ExternalId;
    }
}
