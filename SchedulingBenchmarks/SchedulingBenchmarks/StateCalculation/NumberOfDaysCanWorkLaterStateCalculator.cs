using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class NumberOfDaysCanWorkLaterStateCalculator : IStateCalculator<NumberOfDaysCanWorkLaterStateCalculator.Result>
    {
        private readonly Range _schedulePeriod;

        public NumberOfDaysCanWorkLaterStateCalculator(Range schedulePeriod)
        {
            _schedulePeriod = schedulePeriod;
        }

        public Result CalculateState(Person person, int day)
        {
            var length = Math.Max(person.WorkSchedule.MaxConsecutiveShifts, _schedulePeriod.ExclusiveEnd);
            var numberOfDaysCanWorkLater = 0;
            var consecutiveShiftCount = person.State.ConsecutiveShiftCount;
            if (person.Assignments.AllRounds.ContainsKey(day - 1))
            {
                consecutiveShiftCount++;
            }

            for (int i = day + 1; i < length; i++)
            {
                if (person.Availabilities[i] && consecutiveShiftCount + 1 + i < person.WorkSchedule.MaxConsecutiveShifts)
                {
                    numberOfDaysCanWorkLater++;
                }
            }

            return new Result(numberOfDaysCanWorkLater);
        }

        public Result InitializeState(Person person)
        {
            return CalculateState(person, 0);
        }

        public class Result : IStateCalculatorResult
        {
            public int NumberOfDaysCanWorkLater { get; }

            public Result(int numberOfDaysCanWorkLater)
            {
                NumberOfDaysCanWorkLater = numberOfDaysCanWorkLater;
            }

            public void Apply(Person person)
            {
                person.State.NumberOfDaysCanWorkLater = NumberOfDaysCanWorkLater;
            }
        }
    }
}
