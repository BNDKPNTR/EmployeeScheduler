using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Assignment
    {
        public Person Person { get; }
        public int Day { get; }
        public Shift Shift { get; }

        public Assignment(Person person, int day, Shift shift)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            Day = day;
            Shift = shift ?? throw new ArgumentNullException(nameof(shift));
        }

        public override string ToString() => $"{Day} - {Person.ExternalId} - {Shift.Id}";
    }
}
