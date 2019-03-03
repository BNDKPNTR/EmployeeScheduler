using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Assignment
    {
        public Person Person { get; }
        public int TimeSlot { get; }
        public Shift Shift { get; }

        public Assignment(Person person, int timeSlot, Shift shift)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            TimeSlot = timeSlot;
            Shift = shift ?? throw new ArgumentNullException(nameof(shift));
        }

        public override string ToString() => $"{TimeSlot} - {Person.Id} - {Shift.Id}";
    }
}
