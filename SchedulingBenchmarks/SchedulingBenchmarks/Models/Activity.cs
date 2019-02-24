using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class Activity
    {
        public int Id { get; set; }
        public string Name { get; }

        public Activity(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString() => $"{Id} - {Name}";
    }
}
