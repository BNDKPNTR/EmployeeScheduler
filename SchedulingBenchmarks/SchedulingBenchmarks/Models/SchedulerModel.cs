using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Models
{
    class SchedulerModel
    {
        public Range SchedulePeriod { get; }
        public List<Person> People { get; }
        public Dictionary<int, Demand[]> Demands { get; }
        public Calendar Calendar { get; }
        public ParallelOptions ParallelOptions { get; }

        public SchedulerModel(
            Range schedulePeriod, 
            List<Person> people, 
            Dictionary<int, Demand[]> demands, 
            Calendar calendar, 
            ParallelOptions parallelOptions)
        {
            SchedulePeriod = schedulePeriod;
            People = people ?? throw new ArgumentNullException(nameof(people));
            Demands = demands ?? throw new ArgumentNullException(nameof(demands));
            Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            ParallelOptions = parallelOptions ?? throw new ArgumentNullException(nameof(parallelOptions));
        }
    }
}
