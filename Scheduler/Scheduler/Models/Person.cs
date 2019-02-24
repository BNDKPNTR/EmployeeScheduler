using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Models
{
    class Person
    {
        public int Id { get; }
        public string Name { get; }
        public State State { get; }
        public TimeSlotDependentCollection<bool> Availabilities { get; }
        public bool Available => Availabilities.Current;
        public Dictionary<int, Assignment> Assignments { get; }

        public Person(int id, string name, State state, TimeSlotDependentCollection<bool> availabilities, Dictionary<int, Assignment> assignments)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
            Assignments = assignments ?? throw new ArgumentNullException(nameof(assignments));
        }

        public override string ToString() => Name;
    }
}
