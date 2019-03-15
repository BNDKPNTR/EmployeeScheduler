using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.StateCalculation
{
    class PossibleFutureWorkDayCountStateCalculator : IStateCalculator<PossibleFutureWorkDayCountStateCalculator.Result>
    {
        public Result CalculateState(Person person, StateTriggers triggers, int day)
        {
            if (triggers.WorkedOnPreviousDay)
            {
                return new Result(person.State.PossibleFutureWorkDayCount - 1);
            }

            return new Result(CalculatePossibleFutureWorkDayCount(person, day));
        }

        public Result InitializeState(Person person)
        {
            return new Result(CalculatePossibleFutureWorkDayCount(person, 0));
        }

        private int CalculatePossibleFutureWorkDayCount(Person person, int day)
        {
            var possibleFutureWorkDayCount = 0;
            var length = day + person.WorkSchedule.MaxConsecutiveShifts;

            for (int i = day; i < length; i++)
            {
                if (i < person.Availabilities.Length)
                {
                    if (person.Availabilities[i])
                    {
                        possibleFutureWorkDayCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    possibleFutureWorkDayCount++;
                }
            }

            return possibleFutureWorkDayCount;
        }

        public class Result : IStateCalculatorResult
        {
            public int PossibleFutureWorkDayCount { get; }

            public Result(int numberOfDaysCanWorkLater)
            {
                PossibleFutureWorkDayCount = numberOfDaysCanWorkLater;
            }

            public void Apply(State state)
            {
                state.PossibleFutureWorkDayCount = PossibleFutureWorkDayCount;
            }
        }
    }
}
