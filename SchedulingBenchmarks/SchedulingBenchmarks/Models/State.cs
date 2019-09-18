using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SchedulingBenchmarks.Models
{
    class State : ICloneable
    {
        public int TotalWorkTime { get; set; }
        public int ConsecutiveWorkDayCount { get; set; }
        public int ConsecutiveDayOffCount { get; set; }
        public bool WorkedOnWeeked { get; set; }
        public int WorkedWeekendCount { get; set; }
        public Dictionary<Shift, int> ShiftWorkedCount { get; set; }
        public int PossibleFutureWorkDayCount { get; set; }

        public State Clone()
        {
            return new State
            {
                TotalWorkTime = TotalWorkTime,
                ConsecutiveWorkDayCount = ConsecutiveWorkDayCount,
                ConsecutiveDayOffCount = ConsecutiveDayOffCount,
                WorkedOnWeeked = WorkedOnWeeked,
                WorkedWeekendCount = WorkedWeekendCount,
                ShiftWorkedCount = new Dictionary<Shift, int>(ShiftWorkedCount),
                PossibleFutureWorkDayCount = PossibleFutureWorkDayCount
            };
        }

        object ICloneable.Clone() => Clone();

        public void Restore(State state)
        {
            TotalWorkTime = state.TotalWorkTime;
            ConsecutiveWorkDayCount = state.ConsecutiveWorkDayCount;
            ConsecutiveDayOffCount = state.ConsecutiveDayOffCount;
            WorkedOnWeeked = state.WorkedOnWeeked;
            WorkedWeekendCount = state.WorkedWeekendCount;
            ShiftWorkedCount = state.ShiftWorkedCount;
            PossibleFutureWorkDayCount = state.PossibleFutureWorkDayCount;
        }
    }
}
